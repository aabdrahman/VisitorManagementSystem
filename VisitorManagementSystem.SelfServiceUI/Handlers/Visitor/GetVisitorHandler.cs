using Entities.Response;
using Shared.DataTransferObjects;
using System.Net.Http;
using System.Text.Json;

namespace VisitorManagementSystem.SelfServiceUI.Handlers.Visitor;

public class GetVisitorHandler
{
    private HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public GetVisitorHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        //_httpClien    t = httpClientFactory.CreateClient("SecureApiClient");
    }

    public async Task<(string respMsg, VisitorDto? respDetails)> Handle(string phoneNumber)
    {
        try
        {
            _httpClient = _httpClientFactory.CreateClient("SecureApiClient");
            var response = await _httpClient.GetAsync($"api/visitor/{phoneNumber}");

            if(!response.IsSuccessStatusCode)
            {
                return (respMsg:$"Error Fetching Details. Status Code: {(int)response.StatusCode}", null);
            }

            var responseDetails = await response.Content.ReadAsStringAsync();

            var responseDet = JsonSerializer.Deserialize<Response>(responseDetails, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            var respData = JsonSerializer.Serialize(responseDet?.ResponseData);

            return (respMsg: "Success", respDetails: JsonSerializer.Deserialize<VisitorDto>(respData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }));

        }
        catch (Exception ex)
        {
            return (respMsg: "An Error Occurred Fetching", null);
        }

    }
}
