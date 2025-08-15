using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Identity.Presentation.Controllers.Register;

/// <summary>
///     Contains all error definitions for the register endpoint
/// </summary>
public partial class RegisterController
{
    /// <summary>
    ///     A user with the given name already exists
    /// </summary>
    /// <param name="username">The username which is the source of the error</param>
    /// <returns>StatusCode: 409</returns>
    private ObjectResult UsernameConflict(string username)
    {
        return Problem(
            title: "Username already exists",
            detail: $"A user with the name '{username}' already exists",
            statusCode: StatusCodes.Status409Conflict
        );
    }

    /// <summary>
    ///     The  username does not meet our standards
    /// </summary>
    /// <param name="username">The username which is the source of the error</param>
    /// <returns>StatusCode: 400</returns>
    private ObjectResult InvalidUsername(string username)
    {
        return Problem(
            title: "Invalid username",
            detail: $"The username '{username}' does not fulfill requirements",
            statusCode: StatusCodes.Status400BadRequest
        );
    }

    /// <summary>
    ///     The password does not meet our standards
    /// </summary>
    /// <returns>StatusCode: 400</returns>
    private ObjectResult InvalidPassword()
    {
        return Problem(
            title: "Invalid password",
            detail: "The password does not fulfill requirements",
            statusCode: StatusCodes.Status400BadRequest
        );
    }
}