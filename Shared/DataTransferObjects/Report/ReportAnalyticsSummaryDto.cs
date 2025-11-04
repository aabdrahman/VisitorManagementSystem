namespace Shared.DataTransferObjects.Report;

public record class ReportAnalyticsSummaryDto
(
    List<ReportFilterDto> ReportByVisitStatus,
    List<ReportFilterDto> ReportByVisitRegistrationType,
    List<ReportFilterDto> ReportByVisitType
);
