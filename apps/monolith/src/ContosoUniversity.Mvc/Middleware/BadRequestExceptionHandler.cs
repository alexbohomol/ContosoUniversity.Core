namespace ContosoUniversity.Mvc.Middleware;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

public class BadRequestExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "ValidationFailure",
            Title = "Validation error",
            Detail = "One or more validation errors has occurred",
            Problems = validationException.Errors.Select(x => new
            {
                x.PropertyName,
                x.ErrorMessage
            })
        }, cancellationToken);

        return true;
    }
}

