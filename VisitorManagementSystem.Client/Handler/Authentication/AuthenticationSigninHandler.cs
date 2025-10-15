using Blazored.LocalStorage;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Authentication;

public class AuthenticationSigninHandler
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public AuthenticationSigninHandler(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.OpenClientKey);
        _localStorageService = localStorageService;
    }

    public async Task<(bool, string)> Handle(UserToLoginDto userToLogin)
    {
        try
        {
            var loginResponse = await _httpClient.PostAsJsonAsync("api/authentication/login", userToLogin);
            var responseContent = await loginResponse.Content.ReadAsStringAsync();

            if(loginResponse.StatusCode != HttpStatusCode.OK)
            {
                return (false, responseContent);
            }
            var tokenDetails = JsonSerializer.Deserialize<TokenDto>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            await _localStorageService.SetItemAsync<TokenDto>(ClientHelper.StorageKey, tokenDetails);

            return (true, "Login Success");

        }
        catch (Exception ex)
        {
            return (false,"An Error Occurred.");
            throw;
        }
    }
}
