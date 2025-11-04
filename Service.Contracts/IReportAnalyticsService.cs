using Shared.DataTransferObjects.Report;

namespace Service.Contracts;

public interface IReportAnalyticsService
{
    Task<ReportAnalyticsSummaryDto> GetAnalyticsReport(ReportAnalyticsBoundaryDto reportAnalyticsBoundary);
}
