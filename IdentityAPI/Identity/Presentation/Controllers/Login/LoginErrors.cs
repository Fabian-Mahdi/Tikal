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
    private ObjectResult InvalidCredentials()
    {
        return Problem(
            title: "Invalid credentials",
            detail: "Invalid username or password provided",
            statusCode: StatusCodes.Status401Unauthorized
        );
    }
}