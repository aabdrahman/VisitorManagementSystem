using Shared.DataTransferObjects;
using System.Net.Http.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.UserManagement;

public class RegisterUserHandler
{
    private readonly HttpClient _httpClient;

    public RegisterUserHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<(bool, string)> Handle(UserForCreationDto user)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/authentication/RegisterUser", user);

            var responseContent = await response.Content.ReadAsStringAsync();

            return (response.IsSuccessStatusCode, responseContent);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
