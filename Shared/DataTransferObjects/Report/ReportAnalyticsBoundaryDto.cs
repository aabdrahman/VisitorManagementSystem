namespace Shared.DataTransferObjects.Report;

public class ReportAnalyticsBoundaryDto
{
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public bool IsValid => StartDate <= EndDate;
}
