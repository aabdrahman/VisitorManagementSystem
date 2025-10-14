
using Blazored.LocalStorage;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Text.Json;
using VisitorManagementSystem.Client.Handler.Authentication;
using VisitorManagementSystem.Presentation.Helpers;

namespace VisitorManagementSystem.Client.AuthenticationProvider;

public class AuthStateHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;
    private readonly RefreshTokenHandler _refreshTokenHandler;
    private readonly HttpClient _httpClient;
    private TokenDto? _tokenDto;

    public AuthStateHandler(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory, RefreshTokenHandler refreshTokenHandler)
    {
        _localStorageService = localStorageService;
        _refreshTokenHandler = refreshTokenHandler;
        _httpClient = httpClientFactory.CreateClient(ClientHelper.OpenClientKey);
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey, cancellationToken);

        var result = await base.SendAsync(request, cancellationToken);

        if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            if(token is not null)
            {
                try
                {
                    //var refreshTokenResp = await _httpClient.PostAsJsonAsync("api/authentication/refresh", token, cancellationToken);

                    //refreshTokenResp.EnsureSuccessStatusCode();

                    //var responseContent = await refreshTokenResp.Content.ReadAsStringAsync();

                    //_tokenDto = JsonSerializer.Deserialize<TokenDto>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? null;

                    await _refreshTokenHandler.Handle(); //Calls the refresh token handler. This sets the new token to the local storage.

                    _tokenDto = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey, cancellationToken); //Fetches from the local strage to confirm that the refresh was successful

                    if(_tokenDto is not null)
                    {
                        await _localStorageService.SetItemAsync(ClientHelper.StorageKey, _tokenDto, cancellationToken);
                    }
                }
                catch (HttpRequestException ex)
                {

                    throw;
                }

            }


        }

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenDto?.AccessToken);

        result = await base.SendAsync(request, cancellationToken);

        return result;
    }
}
