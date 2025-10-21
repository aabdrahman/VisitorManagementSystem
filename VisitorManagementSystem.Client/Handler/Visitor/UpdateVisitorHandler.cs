using Entities.Response;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Visitor;

public class UpdateVisitorHandler
{
    private HttpClient _httpClient;

    public UpdateVisitorHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<Response?> Handle(VisitorDto UpdatedVisitor)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/visitor/update-visitor", UpdatedVisitor);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseBody = JsonSerializer.Deserialize<Response?>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? null;

            return responseBody;

        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
