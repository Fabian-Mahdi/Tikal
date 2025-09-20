using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tikal.Presentation.Accounts.Controllers.CreateAccount;

/// <summary>
///     Contains all error definitions for the POST /accounts endpoint
/// </summary>
public partial class CreateAccountController
{
    /// <summary>
    ///     An account for the current user already exists
    /// </summary>
    /// <param name="id">The id which is the source of the error</param>
    /// <returns>StatusCode: 409</returns>
    private ObjectResult AccountExists(string id)
    {
        return Problem(
            title: "Account already exists",
            detail: $"An account with the id '{id}' already exists.",
            statusCode: StatusCodes.Status409Conflict
        );
    }
}