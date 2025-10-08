using Blazored.LocalStorage;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Text.Json;

namespace VisitorManagementSystem.SelfServiceUI.Handlers.Authentication;

public class TokenHandler
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public TokenHandler(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory)
    {
        _localStorageService = localStorageService;
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<TokenDto?> Handle()
    {
        var userToLogin = new UserToLoginDto() { UserName = "SYSTEM.SYSTEM", Password = "String$22" };

        var getTokenResp = await _httpClientFactory.CreateClient("ApiClient").PostAsJsonAsync("api/authentication/login", userToLogin);

        getTokenResp.EnsureSuccessStatusCode();

        var tokenDetailsContent = await getTokenResp.Content.ReadAsStringAsync();

        var tokenDetails = JsonSerializer.Deserialize<TokenDto>(tokenDetailsContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        if(tokenDetails is not null)
        {
            await _localStorageService.SetItemAsync<TokenDto>("access-token", tokenDetails);
        }

        return tokenDetails;
    }
}
