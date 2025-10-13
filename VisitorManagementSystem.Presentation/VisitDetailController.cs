using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;
using VisitorManagementSystem.Presentation.ActionFilters;

namespace VisitorManagementSystem.Presentation;

[Route("api/visitdetail")]
public class VisitDetailController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public VisitDetailController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    [Authorize(Policy = "ReceptionistPolicy")]
    [EnableRateLimiting(policyName: "SpecialPolicy")]
    public async Task<IActionResult> GetAllVisits([FromQuery] VisitDetailRequestParameter visitDetailRequestParameter)
    {
        var response = await _serviceManager.VisitDetailService.GetAllVisits(visitDetailRequestParameter, trackChanges: false, ignoreQueryFilter: true);
        // Request.Headers.Append("X-Pagination", JsonSerializer.Serialize(response.metaData));
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(response.metaData));
        return Ok(response.visits);
    }

    [HttpGet("{VisitorIdentificationNumber}", Name = "GetById")]
    [Authorize(Policy = "ReceptionistPolicy")]
    [EnableRateLimiting(policyName: "SpecialPolicy")]
    public async Task<IActionResult> GetVisitDetailByVisitorIdentificationNumber(string VisitorIdentificationNumber)
    {
        var result = await _serviceManager.VisitDetailService.GetVisitDetailsByIdentificationNumber(VisitorIdentificationNumber, trackChanges: false, ignoreQueryFilter: true);

        return Ok(result);
    }

    [HttpPost("scheduleVisit")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> ScheduleVisit([FromBody] ScheduleVisitDetailDto scheduleVisitDetail)
    {
        var result = await _serviceManager.VisitDetailService.ScheduleVisit(scheduleVisitDetail);

        return CreatedAtRoute("GetById", new { VisitorIdentificationNumber = result.VisitorIdentificationNumber }, result);
        //return Ok(result);
    }

    [HttpPost("update-status")]
    [Authorize(Policy = "ReceptionistPolicy")]
    public async Task<IActionResult> UpdateVisitStatus([FromBody] UpdateVisitStatusDto updateVisitStatus)
    {
        var response = await _serviceManager.VisitDetailService.UpdateStatus(updateVisitStatus);

        return Ok(response);
    }

    [HttpPost("create-walkin")]
    [Authorize(Policy = "ReceptionistPolicy")]
    public async Task<IActionResult> CreateWalkIn([FromBody] CreateVisitDetailDto createVisitDetail)
    {
        var result = await _serviceManager.VisitDetailService.CreateVisit(createVisitDetail);

        return Ok(result);
    }

    [HttpPut("checkIn")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Authorize(Policy = "ReceptionistPolicy")]
    [EnableRateLimiting(policyName: "SpecialPolicy")]
    public async Task<IActionResult> CheckInVisitor([FromBody] VisitorDetailsCheckInDto visitorDetailsCheckIn)
    {
        var result = await _serviceManager.VisitDetailService.UpdateCheckIn(visitorDetailsCheckIn);

        return Ok(result);
    }

    [HttpPut("checkOut")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Authorize(Policy = "ReceptionistPolicy")]
    [EnableRateLimiting(policyName: "SpecialPolicy")]
    public async Task<IActionResult> CheckOutVisitor([FromBody] VisitorDetailsCheckInDto checkOutDetails)
    {
        var result = await _serviceManager.VisitDetailService.UpdateCheckOut(checkOutDetails);

        return Ok(result);
    }

}
