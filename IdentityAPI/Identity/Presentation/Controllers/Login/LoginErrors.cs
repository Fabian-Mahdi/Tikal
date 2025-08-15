using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Identity.Presentation.Controllers.Login;

/// <summary>
///     Contains all error definitions for the login endpoint
/// </summary>
public partial class LoginController
{
    /// <summary>
    ///     The provided credentials are invalid
    /// </summary>
    /// <returns>StatusCode: 401</returns>
    private static ObjectResult InvalidCredentials()
    {
        ProblemDetails problemDetails = new()
        {
            Title = "Invalid credentials",
            Detail = "Invalid password or username provided",
            Status = StatusCodes.Status401Unauthorized
        };

        return new ObjectResult(problemDetails);
    }
}