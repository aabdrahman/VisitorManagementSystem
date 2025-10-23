using Shared.DataTransferObjects;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.UserManagement;

public class GetRolesHandler
{
    private HttpClient _httpClient;

    public GetRolesHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<IEnumerable<RoleDto>> Handle()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/authentication/getRoles");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<IEnumerable<RoleDto>>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? [];
        }
        catch (Exception ex)
        {
            return [];
        }
    }
}
