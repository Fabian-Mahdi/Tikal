using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Tikal.App.Extensions;

public static class HttpContextExtensions
{
    public static async Task WriteProblem(this HttpContext context, int statusCode, string title, string detail)
    {
        ProblemDetailsFactory problemFactory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        ProblemDetails problem = problemFactory.CreateProblemDetails(
            context,
            statusCode,
            title,
            detail: detail
        );

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(problem);
    }
}