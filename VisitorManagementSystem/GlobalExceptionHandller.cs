using Contracts;
using Entities.ErrorModels;
using Entities.Exceptions;
using Entities.Response;
using Microsoft.AspNetCore.Diagnostics;

namespace VisitorManagementSystem;

public class GlobalExceptionHandller : IExceptionHandler
{
    private readonly ILoggerManager _loggerManager;

    public GlobalExceptionHandller(ILoggerManager loggerManager)
    {
        _loggerManager = loggerManager;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        _loggerManager.LogError($"An Error Occurred. {exception.Message}");

        var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

        var errorResponse = new ErrorDetails();

        errorResponse.StatusCode = contextFeature?.Error switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        errorResponse.Message = contextFeature?.Error.Message!;
        errorResponse.ErrorDescription = exception.InnerException?.Message;

        //_loggerManager.LogError(errorResponse.ToString());
        var response = Response.CreateErrorResponse(errorResponse, exception.Message, contextFeature.Error.GetType().ToString());
        _loggerManager.LogError(response.ToString());

        await httpContext.Response.WriteAsync(response.ToString());

        return true;
    }
}
