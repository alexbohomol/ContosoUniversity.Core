namespace ContosoUniversity.Mvc.Middleware;

using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

public class EntityNotFoundExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not EntityNotFoundException notFoundException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            Status = StatusCodes.Status404NotFound,
            Type = "Not Found",
            Title = "Entity Not Found",
            Detail = notFoundException.Message
        }, cancellationToken);

        return true;
    }
}
