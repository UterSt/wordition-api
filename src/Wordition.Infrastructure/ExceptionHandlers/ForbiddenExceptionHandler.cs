using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wordition.Domain.Exceptions;

namespace Wordition.Infrastructure.ExceptionHandlers;

public class ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger) : ExceptionHandlerBase, IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException)
            return false;
        
        logger.LogWarning(exception, "Attempt to obtain a prohibited resource");
        
        await WriteResponse(httpContext, "You do not have sufficient rights to access this resource", StatusCodes.Status403Forbidden, cancellationToken);
        
        return true;
    }
}