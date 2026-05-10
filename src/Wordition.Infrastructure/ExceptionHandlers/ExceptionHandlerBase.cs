using Microsoft.AspNetCore.Http;
using Wordition.Application.DTO;

namespace Wordition.Infrastructure.ExceptionHandlers;

public abstract class ExceptionHandlerBase
{
    protected async Task WriteResponse(HttpContext httpContext, string message, int statusCode, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = statusCode;
        
        await httpContext.Response.WriteAsJsonAsync(new ErrorResponse()
        {
            Message = message,
            StatusCode = statusCode,
            TraceId = httpContext.TraceIdentifier,
        }, cancellationToken);
    }
}