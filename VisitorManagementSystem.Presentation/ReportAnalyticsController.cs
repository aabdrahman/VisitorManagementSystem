using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects.Report;

namespace VisitorManagementSystem.Presentation;

[Route("api/reportanalytics")]
[ApiController]
[Authorize(Policy = "AdminPolicy")]
public class ReportAnalyticsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    public ReportAnalyticsController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("get-main-analytics")]
    public async Task<IActionResult> GetReportAnalytics([FromQuery] ReportAnalyticsBoundaryDto reportAnalyticsBoundaryDto)
    {
        try
        {
            if(!reportAnalyticsBoundaryDto.IsValid)
            {
                return StatusCode(400, "Start Date cannot be greater than end date");
            }

            var response = await _serviceManager.ReportAnalyticsService.GetAnalyticsReport(reportAnalyticsBoundaryDto);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
