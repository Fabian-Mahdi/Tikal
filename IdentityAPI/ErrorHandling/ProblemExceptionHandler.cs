using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.ErrorHandling;

public class ProblemExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService problemDetailsService;

    public ProblemExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        this.problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not ProblemException problemException)
        {
            return true;
        }

        ProblemDetails problemDetails = new()
        {
            Title = problemException.Title,
            Status = (int)problemException.Status,
            Extensions = new Dictionary<string, object?>
            {
                { "errors", problemException.Errors }
            }
        };

        httpContext.Response.StatusCode = (int)problemException.Status;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });
    }
}