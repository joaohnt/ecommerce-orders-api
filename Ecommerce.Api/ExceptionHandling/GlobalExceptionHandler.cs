using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problem = exception switch
        {
            InvalidOperationException => new ProblemDetails
            {
                Title = "Ocorreu um erro.",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            },
            ArgumentOutOfRangeException => new ProblemDetails
            {
                Title = "Ocorreu um erro.",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            },
            ArgumentNullException => new ProblemDetails
            {
                Title = "Ocorreu um erro.",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            },
            KeyNotFoundException => new ProblemDetails
            {
                Title = "Ocorreu um erro.",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            },
            ArgumentException => new ProblemDetails
            {
                Title = "Ocorreu um erro.",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            },
            _ => new ProblemDetails
            {
                Title = "Internal server error",
                Detail = "Ocorreu um erro inesperado.",
                Status = StatusCodes.Status500InternalServerError
            }
        };

        httpContext.Response.StatusCode = problem.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}