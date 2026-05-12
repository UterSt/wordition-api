using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wordition.Domain.Exceptions;

namespace Wordition.Infrastructure.ExceptionHandlers;

public class NotUniqueLoginHandler(ILogger<NotUniqueLoginHandler> logger) :  ExceptionHandlerBase, IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotUniqueLoginException)
            return false;
        
        logger.LogInformation(exception, "Not a unique login");
        
        await WriteResponse(httpContext, "This login already exists",StatusCodes.Status409Conflict , cancellationToken);
        return true;
    }
}