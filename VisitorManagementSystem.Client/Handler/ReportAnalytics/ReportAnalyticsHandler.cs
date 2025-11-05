using Shared.DataTransferObjects.Report;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.ReportAnalytics;

public class ReportAnalyticsHandler
{
    private readonly HttpClient _httpClient;

    public ReportAnalyticsHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(ClientHelper.SecureClientKey);
    }

    public async Task<ReportAnalyticsSummaryDto?> Handle(ReportAnalyticsBoundaryDto reportAnalyticsBoundary)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/reportanalytics/get-main-analytics?startDate={reportAnalyticsBoundary.StartDate}&enddate={reportAnalyticsBoundary.EndDate}");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var reportAnalytics = JsonSerializer.Deserialize<ReportAnalyticsSummaryDto>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true}) ?? null;

            return reportAnalytics;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
