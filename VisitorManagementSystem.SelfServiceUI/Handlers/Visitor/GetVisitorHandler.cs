using Entities.Response;
using Shared.DataTransferObjects;
using System.Text.Json;

namespace VisitorManagementSystem.SelfServiceUI.Handlers.Visitor;

public class GetVisitorHandler
{
    private HttpClient _httpClient;

    public GetVisitorHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
    }

    public async Task<(string respMsg, VisitorDto? respDetails)> Handle(string phoneNumber)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/visitor?phoneNumber={phoneNumber}");

            if(!response.IsSuccessStatusCode)
            {
                return (respMsg:$"Error Fetching Details. Status Code: {(int)response.StatusCode}", null);
            }

            var responseDetails = await response.Content.ReadAsStringAsync();

            var responseDet = JsonSerializer.Deserialize<Response>(responseDetails);

            var resp = (VisitorDto)responseDet.ResponseData;

            return (respMsg: "Success", respDetails: resp);

        }
        catch (Exception ex)
        {
            return (respMsg: "An Error Occurred Fetching", null);
        }

    }
}
