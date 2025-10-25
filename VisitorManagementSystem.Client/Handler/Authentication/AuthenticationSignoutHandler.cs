using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using VisitorManagementSystem.Client.AuthenticationProvider;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Authentication;

public class AuthenticationSignoutHandler
{
    private ILocalStorageService _localStorageService;
    private AuthenticationStateProvider _authStateProvider;
    private HttpClient _httpClient;
    public AuthenticationSignoutHandler(ILocalStorageService localStorageService, AuthenticationStateProvider authStateProvider, IHttpClientFactory httpClientFactory)
    {
        _localStorageService = localStorageService;
        _authStateProvider = authStateProvider;
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<bool> Handle()
    {
        await _localStorageService.RemoveItemAsync(ClientHelper.StorageKey);
        await ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        _httpClient.DefaultRequestHeaders.Authorization = default;

        return true;
    }
}
