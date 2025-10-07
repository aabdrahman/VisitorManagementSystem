using Entities.Response;
using Shared.DataTransferObjects;
using System.Text.Json;

namespace VisitorManagementSystem.SelfServiceUI.Handlers.VisitDetail;

public class GetByIdentificationNumberHandler
{
    private HttpClient _httpClient;

    public GetByIdentificationNumberHandler(IHttpClientFactory httpClientFactory)
    {
            _httpClient = httpClientFactory.CreateClient("SecureApiClient");
    }

    public async Task<(string respMsg, VisitDetailDto? visitDetail)> Handle(string VisitorIdentificationNumber)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/visitdetail/{VisitorIdentificationNumber}");

            if(!response.IsSuccessStatusCode)
            {
                var responseCont = await response.Content.ReadAsStringAsync();

                var responseBody = JsonSerializer.Deserialize<Response>(responseCont, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return (respMsg: responseBody?.ResponseMessage ?? "", null);
            }
            else
            {
                var responseCont = await response.Content.ReadAsStringAsync();

                var responseBody = JsonSerializer.Deserialize<VisitDetailDto>(responseCont, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return (respMsg: "Success", visitDetail: responseBody);
            }
        }
        catch (Exception ex)
        {
            //return (ex.Message, new VisitDetailDto() { HostName = "Test", PurposeOfVisit = "Test", VisitDate = DateOnly.FromDateTime(DateTime.Now), VisitorEmailAddress = "test@mail.com", VisitorGender = "Male", VisitorIdentificationNumber = VisitorIdentificationNumber, VisitorName = "Test", VisitorPhoneNumber = "Test" });
            return (ex.Message, null);
        }
    }
}
