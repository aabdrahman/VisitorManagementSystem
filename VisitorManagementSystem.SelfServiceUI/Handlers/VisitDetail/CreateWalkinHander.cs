using Entities.Response;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Text.Json;

namespace VisitorManagementSystem.SelfServiceUI.Handlers.VisitDetail;

public class CreateWalkinHander
{
    private readonly HttpClient _httpClient;

    public CreateWalkinHander(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("SecureApiClient");
    }

    public async Task<Response?> Handle(CreateVisitDetailDto createVisitDetail)
    {
        var response = await _httpClient.PostAsJsonAsync("api/visitdetail/create-walkin", createVisitDetail);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var responseBody = JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return responseBody;
    }
}
