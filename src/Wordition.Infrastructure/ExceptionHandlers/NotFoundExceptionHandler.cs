using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wordition.Domain.Exceptions;

namespace Wordition.Infrastructure.ExceptionHandlers;

public class NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger) : ExceptionHandlerBase, IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException)
            return false;
        
        logger.LogWarning(exception, "Resource not found");
        
        await WriteResponse(httpContext, "The requested resource was not found", StatusCodes.Status404NotFound, cancellationToken);
        return true;
    }
}