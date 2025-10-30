using Entities.Response;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Visit;

public class UpdateVisitStatusHandler
{
    private HttpClient _httpClient;

    public UpdateVisitStatusHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<(bool, string)> Handle(UpdateVisitStatusDto updateVisit)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/visitdetail/update-status", updateVisit);

            response.EnsureSuccessStatusCode();

            var respContent = await response.Content.ReadAsStringAsync();

            var responseBody = JsonSerializer.Deserialize<Response>(respContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            var successStatus = responseBody.ResponseCode.Contains("00", StringComparison.CurrentCultureIgnoreCase);

            var responseMessage = responseBody.ResponseMessage;

            return (successStatus, responseMessage);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
