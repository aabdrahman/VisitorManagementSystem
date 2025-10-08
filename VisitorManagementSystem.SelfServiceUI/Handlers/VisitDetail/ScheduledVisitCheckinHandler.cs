using Entities.Response;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Text.Json;

namespace VisitorManagementSystem.SelfServiceUI.Handlers.VisitDetail;

public class ScheduledVisitCheckinHandler
{
    private readonly HttpClient _httpClient;

    public ScheduledVisitCheckinHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("SecureApiClient");
    }

    public async Task<Response?> Handle(UpdateVisitStatusDto updateVisitStatus)
    {
        var response = await _httpClient.PostAsJsonAsync("api/visitdetail/update-status", updateVisitStatus);

        if(!response.IsSuccessStatusCode)
        {
            return null ;
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var responseBody = JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return responseBody;
    }
}
