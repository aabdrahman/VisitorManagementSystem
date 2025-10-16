using Entities.Response;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Visit;

public class CheckinHandler
{
    private HttpClient _httpClient;

    public CheckinHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }


    public async Task<(SuccessfulCheckInDetailsDto?, string)> Handle(VisitorDetailsCheckInDto checkInDetails)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/visitdetail/checkIn", checkInDetails);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var checkInSuccessResponse = JsonSerializer.Deserialize<SuccessfulCheckInDetailsDto>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return (checkInSuccessResponse, $"Checked In At: {checkInSuccessResponse?.CheckInTime}");
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    var checkOut429ErrorResponse = JsonSerializer.Deserialize<string>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    string retryAfterHeader = "";
                    if (response.Headers.TryGetValues("Retry-After", out var responseHeaders))
                    {
                        retryAfterHeader = responseHeaders.FirstOrDefault() ?? "";
                    }
                    return (null, $"{checkOut429ErrorResponse ?? $"Too many requests. Retry after: {retryAfterHeader}"}");
                }
                var checkInErrorResponse = JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return (null, $"{checkInErrorResponse?.ResponseMessage}");
            }

        }
        catch (TaskCanceledException)
        {
            return (null, "The request timed out. Please try again later.");
        }
        catch (Exception ex)
        {
            return (null, "An Error Occurred.");
        }
    }
}
