using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using VisitorManagementSystem.Presentation.ActionFilters;

namespace VisitorManagementSystem.Presentation;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public AuthenticationController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordDto changePasswordDetails)
    {
        var result = await _serviceManager.AuthenticationService.ResetPassword(changePasswordDetails);

        return Ok(result);
    }

    [HttpPost("createRole")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Authorize(Roles = "ReceptionistPolicy")]
    public async Task<IActionResult> CreateRole([FromBody] RoleForRegistrationDto roleForRegistration)
    {
        //if(!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}
        //string accessToken;
        //var token = HttpContext.Request.Headers.TryGetValue("Authorization",)

        var headerAuth = HttpContext.Request.Headers["Authorization"].ToString();
        if(string.IsNullOrEmpty(headerAuth))
        {
            return Unauthorized("No Token Passed.");
        }

        var tokenHeaders = headerAuth.Split(' ')[1];
        if(string.IsNullOrEmpty(tokenHeaders))
        {
            return Unauthorized("Empty Token");
        }
        var token = tokenHeaders;

        var result = await _serviceManager.AuthenticationService.CreateRole(roleForRegistration, token);

        if(!result.Succeeded)
        {
            foreach(var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return Created();
    }

    [HttpGet("getRoles")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetRoles()
    {
        var result = await _serviceManager.AuthenticationService.GetRoles();

        return Ok(result);
    }

    [HttpGet("fetch-users")]
    public async Task<IActionResult> GetUsers([FromQuery] UsersRequestParameter requestParameter)
    {
        var users = await _serviceManager.AuthenticationService.GetAllUsers(requestParameter);

        return Ok(users);
    }

    [HttpPost("RegisterUser")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> RegisterUser([FromBody] UserForCreationDto userForCreation)
    {
        var result = await _serviceManager.AuthenticationService.CreateUser(userForCreation);

        if(!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [EnableRateLimiting(policyName: "SpecialPolicy")]
    public async Task<IActionResult> ValidateUser([FromBody] UserToLoginDto userToLogin)
    {
        var result = await _serviceManager.AuthenticationService.ValidateUser(userToLogin);

        if (!result)
            return Unauthorized("Invalid Username or Password.");

        var token = await _serviceManager.AuthenticationService.GenerateToken(populateExp: true);

        return Ok(token);
    }

    [HttpPost("refresh")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [EnableRateLimiting(policyName: "SpecialPolicy")]
    public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
    {
        var result = await _serviceManager.AuthenticationService.GenerateRefreshToken(tokenDto);

        return Ok(result);
    }
}


