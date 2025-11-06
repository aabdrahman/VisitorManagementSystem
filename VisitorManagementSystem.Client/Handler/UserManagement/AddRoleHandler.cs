using Entities.Response;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.UserManagement;

public class AddRoleHandler
{
    private HttpClient _httpClient;

    public AddRoleHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<(bool, string)> Handle(RoleForRegistrationDto newRole)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/authentication/createRole", newRole);
            var responseContent = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "Role Created Successfully.") : (false, responseContent);
        }
        catch(HttpRequestException ex)
        {
            return (false, $"An Error Occurred: {ex.Message}");
        }
        catch (Exception ex)
        {
            return (false, "An Error Occurred.");
        }
    }
}
