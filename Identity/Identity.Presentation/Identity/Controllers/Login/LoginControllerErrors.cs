using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Identity.Controllers.Login;

/// <summary>
///     Contains all error definitions for the POST /login endpoint
/// </summary>
public partial class LoginController
{
    /// <summary>
    ///     The provided credentials were invalid
    /// </summary>
    /// <returns>StatusCode: 401</returns>
    private ObjectResult InvalidCredentials()
    {
        return Problem(
            title: "Invalid credentials",
            detail: "Invalid username or password provided.",
            statusCode: StatusCodes.Status401Unauthorized
        );
    }
}