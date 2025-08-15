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
    private static ObjectResult UsernameConflict(string username)
    {
        ProblemDetails problemDetails = new()
        {
            Title = "Username already exists.",
            Detail = $"A user with the name \"{username}\" already exists.",
            Status = StatusCodes.Status409Conflict
        };

        return new ObjectResult(problemDetails);
    }

    /// <summary>
    ///     The  username does not meet our standards
    /// </summary>
    /// <param name="username">The username which is the source of the error</param>
    /// <returns>StatusCode: 400</returns>
    private static ObjectResult InvalidUsername(string username)
    {
        ProblemDetails problemDetails = new()
        {
            Title = "Invalid username.",
            Detail = $"Username \"{username}\" does not fulfill requirements",
            Status = StatusCodes.Status400BadRequest
        };

        return new ObjectResult(problemDetails);
    }

    /// <summary>
    ///     The password does not meet our standards
    /// </summary>
    /// <returns>StatusCode: 400</returns>
    private static ObjectResult InvalidPassword()
    {
        ProblemDetails problemDetails = new()
        {
            Title = "Invalid password.",
            Detail = "Password does not fulfill requirements.",
            Status = StatusCodes.Status400BadRequest
        };

        return new ObjectResult(problemDetails);
    }
}