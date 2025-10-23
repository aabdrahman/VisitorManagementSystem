using Entities.Model.Helpers;
using Shared.RequestFeatures;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.UserManagement;

public class GetUsersHandler
{
    private HttpClient _httpClient;

    public GetUsersHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<IEnumerable<UserSummaryDetails>> Handle(UsersRequestParameter parameter)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/authentication/fetch-users?NumberOfRecord={parameter.NumberOfRecord}&NumberOfRecordsToSkip={parameter.NumberOfRecordsToSkip}&RoleName={parameter.RoleName}&Username={parameter.Username}");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var userSummaryDetails = JsonSerializer.Deserialize<IEnumerable<UserSummaryDetails>>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? [];

            return userSummaryDetails;
        }
        catch (Exception ex)
        {
            return [];
        }
    }
}
