using Shared.DataTransferObjects;
using System.Net.Http.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Visit;

public class ScheduleVisitHandler
{
    private HttpClient _httpClient;

    public ScheduleVisitHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<(bool, string)> Handle(ScheduleVisitDetailDto scheduleVisitDetail)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/visitdetail/scheduleVisit", scheduleVisitDetail);

            response.EnsureSuccessStatusCode();

            return (true, $"Visit Schedled Successfully.");
        }
        catch(HttpRequestException ex)
        {
            return (false, $"{ex.Message}. Status Code: {ex.StatusCode.ToString()}");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
