using Entities.Response;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Visitor;

public class GetVisitorByPhoneNumberHandler
{
    private HttpClient _httpClient;

    public GetVisitorByPhoneNumberHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<Response?> Handle(string PhoneNumber)
    {
        try
        {
            HttpResponseMessage httpResponse = await _httpClient.GetAsync($"api/visitor/{PhoneNumber}");

            httpResponse.EnsureSuccessStatusCode();

            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Response?>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
