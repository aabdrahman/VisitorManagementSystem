using Shared.DataTransferObjects;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace VisitorManagementSystem.SelfServiceUI.AuthProvider;

public class AuthStateHandler : DelegatingHandler
{
    private readonly IHttpClientFactory _httpClientFactory; 
    public AuthStateHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Fetching in Authentication State Handler....");
        var userToLogin = new UserToLoginDto() { UserName = "SYSTEM.SYSTEM", Password = "String$22" };

        var getTokenResp = await _httpClientFactory.CreateClient("ApiClient").PostAsJsonAsync("api/authentication/login", userToLogin);

        getTokenResp.EnsureSuccessStatusCode();

        var tokenDetailsContent = await getTokenResp.Content.ReadAsStringAsync();
        Console.WriteLine($"Fetching Content: {tokenDetailsContent}");
        var tokenDetails = JsonSerializer.Deserialize<TokenDto>(tokenDetailsContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        //var token = await _localStorageService.GetItemAsync<string>("accessToken");
        if (!string.IsNullOrWhiteSpace(tokenDetails.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenDetails.AccessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
