
using Blazored.LocalStorage;
using Shared.DataTransferObjects;
using System.Net.Http.Headers;
using VisitorManagementSystem.Client.Handler.Authentication;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.AuthenticationProvider;

public class AuthStateHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;
    private readonly RefreshTokenHandler _refreshTokenHandler;
    private TokenDto? _tokenDto;

    public AuthStateHandler(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory, RefreshTokenHandler refreshTokenHandler)
    {
        _localStorageService = localStorageService;
        _refreshTokenHandler = refreshTokenHandler;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _tokenDto = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey, cancellationToken);
        if (_tokenDto is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenDto.AccessToken);
        }
        var result = await base.SendAsync(request, cancellationToken);
        if (result.IsSuccessStatusCode)
        {
            return result;
        }

        if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            if (_tokenDto is not null)
            {
                try
                {
                    await _refreshTokenHandler.Handle(); //Calls the refresh token handler. This sets the new token to the local storage.

                    _tokenDto = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey, cancellationToken); //Fetches from the local strage to confirm that the refresh was successful

                    if(_tokenDto is not null)
                    {
                        await _localStorageService.SetItemAsync(ClientHelper.StorageKey, _tokenDto, cancellationToken);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenDto?.AccessToken);

                        result = await base.SendAsync(request, cancellationToken);
                    }
                }

                catch (HttpRequestException ex)
                {

                    throw;
                }

            }


        }

        return result;
    }
}
