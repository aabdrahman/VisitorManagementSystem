using Entities.Response;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Visit;

public class CheckoutHandler
{
    private readonly HttpClient _httpClient;

    public CheckoutHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<(SuccessfulCheckInDetailsDto?, string)> Handle(VisitorDetailsCheckInDto checkoutDetails)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("api/visitdetail/checkOut", checkoutDetails);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var checkOutSuccessResponse = JsonSerializer.Deserialize<SuccessfulCheckInDetailsDto>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return (checkOutSuccessResponse, $"Checked Out At: {checkOutSuccessResponse?.CheckOutTime}");
            }
            else
            {
                if(response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    var checkOut429ErrorResponse = JsonSerializer.Deserialize<string>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    string retryAfterHeader = "";
                    if(response.Headers.TryGetValues("Retry-After", out var responseHeaders))
                    {
                        retryAfterHeader = responseHeaders.FirstOrDefault() ?? "";
                    }
                    return (null, $"{checkOut429ErrorResponse ?? $"Too many requests. Retry after: {retryAfterHeader}"}");
                }
                var checkOutErrorResponse = JsonSerializer.Deserialize<Response>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return (null, $"{checkOutErrorResponse?.ResponseMessage}");
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
