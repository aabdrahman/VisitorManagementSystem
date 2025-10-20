using Entities.Response;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Visitor;

public class GetVisitorsHandler
{
    private HttpClient _httpClient;

    public GetVisitorsHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<Response?> Handle()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/visitor");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseBody =  JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return responseBody;
        }
        catch (Exception ex)
        {
            return Response.CreateErrorResponse(null, "", "01");
        }
    }
}
