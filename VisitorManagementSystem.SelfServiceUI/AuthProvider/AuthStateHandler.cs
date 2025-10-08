using Blazored.LocalStorage;
using Shared.DataTransferObjects;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using VisitorManagementSystem.SelfServiceUI.Handlers.Authentication;

namespace VisitorManagementSystem.SelfServiceUI.AuthProvider;

public class AuthStateHandler : DelegatingHandler
{
    private readonly TokenHandler _tokenHandler;
    private readonly ILocalStorageService _localStorageService;
    public AuthStateHandler(TokenHandler tokenHandler, ILocalStorageService localStorageService)
    {
        _tokenHandler = tokenHandler;
        _localStorageService = localStorageService;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        var tokenDetails = await _localStorageService.GetItemAsync<TokenDto>("access-token");

        if(tokenDetails is null)
        {
            await _tokenHandler.Handle();
        }

        if (!string.IsNullOrWhiteSpace(tokenDetails?.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenDetails.AccessToken);
        }

        var result = await base.SendAsync(request, cancellationToken);

        if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _tokenHandler.Handle();
            tokenDetails = await _localStorageService.GetItemAsync<TokenDto>("access-token");

            if(tokenDetails is null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenDetails?.AccessToken);
            }
        }

        return result;
    }
}
