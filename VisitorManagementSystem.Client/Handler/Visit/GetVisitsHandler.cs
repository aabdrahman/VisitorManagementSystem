using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Visit;

public class GetVisitsHandler
{
    private readonly HttpClient _httpClient;

    public GetVisitsHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<(IEnumerable<VisitDetailDto>, MetaData?)> Handle(VisitDetailRequestParameter requestParameter)
    {
        try
        {
            Console.WriteLine($"Calling GET enpoint from the handler....");
            var getDetailsResponse = await _httpClient.GetAsync($"api/visitdetail?startDate={requestParameter.startDate}&endDate={requestParameter.endDate}&Status={requestParameter.Status}&pageNumber={requestParameter.pageNumber}&PageSize={requestParameter.PageSize}");
            Console.WriteLine($"GET endpoint returns: {getDetailsResponse.StatusCode.ToString()}");
            getDetailsResponse.EnsureSuccessStatusCode();

            var getDetailsRespContent = await getDetailsResponse.Content.ReadAsStringAsync();
            string paginationHeader = "";

            if (getDetailsResponse.Headers.TryGetValues("X-Pagination", out var responseHeaders))
            {
                paginationHeader = responseHeaders.FirstOrDefault();
            }

            var visits = JsonSerializer.Deserialize<IEnumerable<VisitDetailDto>>(getDetailsRespContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? [];
            var pagination = JsonSerializer.Deserialize<MetaData>(paginationHeader, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? null;

            return (visits, pagination);
        }
        catch (TaskCanceledException)
        {
            return ([], null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ([], null);
        }
    }
}
