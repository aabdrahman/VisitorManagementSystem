using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.DataTransferObjects;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using VisitorManagementSystem.Client.Helpers;
using VisitorManagementSystem.Presentation.Helpers;
using System.Text.Json;

namespace VisitorManagementSystem.Client.AuthenticationProvider;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _httpClient;
    private AuthenticationState _anonymous;
    private TokenDto? _tokenDto;

    public AuthStateProvider(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClientFactory.CreateClient(ClientHelper.BaseUri);
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsPrincipal()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _tokenDto = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey) ?? null;
        Console.WriteLine($"{JsonSerializer.Serialize(_tokenDto)}");

        if(_tokenDto is null)
        {
            return _anonymous;
        }

        var claimsPrincipals = JwtParser.ParseClaimsFromJwt(_tokenDto.AccessToken);

        var expValue = claimsPrincipals.FirstOrDefault(x => x.Type == "exp")?.Value ?? default;

        if(!long.TryParse(expValue, out long expiredTimestamp))
        {
            await _localStorageService.RemoveItemAsync(ClientHelper.StorageKey);
            return _anonymous;
        }

        var expTime = DateTimeOffset.FromUnixTimeSeconds(expiredTimestamp);

        if(DateTimeOffset.UtcNow >= expTime)
        {
            await _localStorageService.RemoveItemAsync(ClientHelper.StorageKey);
            return _anonymous;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenDto.AccessToken);

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claimsPrincipals, authenticationType: "jwtAuthType", nameType: "Username", roleType: ClaimTypes.Role)));
    }
}
