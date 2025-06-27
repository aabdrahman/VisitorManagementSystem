using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OutputCaching;

namespace VisitorManagementSystem.Presentation;

[Route("api/visitor")]
[ApiController]
public class VisitorController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public VisitorController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    //[OutputCache(PolicyName = "300SecondsPolicy")]
    [ResponseCache(CacheProfileName = "300Seconds")]
    public async Task<IActionResult> GetVisitors()
    {
        var result = await _serviceManager.VistorService.GetAll(trackChanges: false, ignoreQueryFilter: true);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> CreateVisitor([FromBody] CreateVisitorDto newVisitor)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _serviceManager.VistorService.CreateVisitor(newVisitor);

        return Ok(response);
    }

    [HttpGet("{phoneNumber}", Name = "GetByPhoneNumber")]
    [Authorize(Policy = "ReceptionistPolicy")]
    //[OutputCache(PolicyName = "300SecondsPolicy")]
    [ResponseCache(CacheProfileName = "300Seconds")]
    public async Task<IActionResult> GetVisitorByPhoneNumber(string phoneNumber)
    {
        var response = await _serviceManager.VistorService.GetVisitor(phoneNumber, trackChanges: false, ignoreQueryFilter: false);


        return Ok(response);
    }
}
