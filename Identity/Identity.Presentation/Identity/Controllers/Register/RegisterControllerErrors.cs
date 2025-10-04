using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Identity.Controllers.Register;

/// <summary>
///     Contains all error definitions for the POST /register endpoint
/// </summary>
public partial class RegisterController
{
    /// <summary>
    ///     A user with the given username already exists
    /// </summary>
    /// <param name="username">The username which is the source of the error</param>
    /// <returns>StatusCode: 409</returns>
    private ObjectResult UsernameExists(string username)
    {
        return Problem(
            title: "Username already exists",
            detail: $"An account with the username '{username}' already exists.",
            statusCode: StatusCodes.Status409Conflict
        );
    }
}