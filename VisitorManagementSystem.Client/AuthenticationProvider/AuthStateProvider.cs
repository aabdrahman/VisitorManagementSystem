using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.DataTransferObjects;
using System.Net.Http.Headers;
using System.Security.Claims;
using VisitorManagementSystem.Client.Helpers;
using System.Text.Json;
using VisitorManagementSystem.Client.Handler.Authentication;

namespace VisitorManagementSystem.Client.AuthenticationProvider;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly RefreshTokenHandler _refreshTokenHandler;
    private readonly HttpClient _httpClient;
    private AuthenticationState _anonymous;
    private TokenDto? _tokenDto;

    public AuthStateProvider(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory, RefreshTokenHandler refreshTokenHandler)
    {
        _localStorageService = localStorageService;
        _refreshTokenHandler = refreshTokenHandler;
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _tokenDto = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey) ?? null;
        Console.WriteLine($"{JsonSerializer.Serialize(_tokenDto)}");

        if(_tokenDto is null)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
            return _anonymous;
        }

        var claimsPrincipals = JwtParser.ParseClaimsFromJwt(_tokenDto.AccessToken);

        var expValue = claimsPrincipals.FirstOrDefault(x => x.Type == "exp")?.Value ?? default;

        if(!long.TryParse(expValue, out long expiredTimestamp))
        {
            await _localStorageService.RemoveItemAsync(ClientHelper.StorageKey);
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
            return _anonymous;
        }

        var expTime = DateTimeOffset.FromUnixTimeSeconds(expiredTimestamp);

        if(DateTimeOffset.UtcNow >= expTime)
        {
            await _localStorageService.RemoveItemAsync(ClientHelper.StorageKey);
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
            return _anonymous;
        }

        var timeToExpiry = (expTime - DateTimeOffset.UtcNow).TotalSeconds;

        if (timeToExpiry <= ClientHelper.GetRefreshTokenWindow)
        {
            try
            {
                await _refreshTokenHandler.Handle();
                _tokenDto = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey);
                if (_tokenDto is null)
                {
                    NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
                    return _anonymous;
                }

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(
                        new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(_tokenDto.AccessToken),
                        "jwtAuthType", "Username", ClaimTypes.Role)))));
            }
            catch
            {
                await _localStorageService.RemoveItemAsync(ClientHelper.StorageKey);
                NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
                return _anonymous;
            }
        }


        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenDto?.AccessToken);


        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claimsPrincipals, authenticationType: "jwtAuthType", nameType: "Username", roleType: ClaimTypes.Role)));
    }

    public async Task NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymous);
        await _localStorageService.RemoveItemAsync(ClientHelper.StorageKey);
        NotifyAuthenticationStateChanged(authState);

    }
}
