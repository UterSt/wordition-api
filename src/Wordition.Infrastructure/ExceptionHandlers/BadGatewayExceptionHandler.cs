using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wordition.Domain.Exceptions;

namespace Wordition.Infrastructure.ExceptionHandlers;

public class BadGatewayExceptionHandler(ILogger<BadGatewayExceptionHandler> logger) : ExceptionHandlerBase,  IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadGatewayException)
            return false;
        
        logger.LogWarning(exception, "Bad Gateway");
        
        await WriteResponse(httpContext, "The server returned an invalid or incomplete response",  StatusCodes.Status502BadGateway, cancellationToken);
        
        return true;
    }
}