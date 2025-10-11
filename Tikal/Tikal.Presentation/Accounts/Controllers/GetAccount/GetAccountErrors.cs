using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tikal.Domain.Accounts;

namespace Tikal.Presentation.Accounts.Controllers.GetAccount;

/// <summary>
///     Contains all error definitions for the GET /accounts endpoint
/// </summary>
public partial class GetAccountController
{
    /// <summary>
    ///     There is no registered <see cref="Account" /> for the current user
    /// </summary>
    /// <returns>StatusCode: 404</returns>
    private ObjectResult NoAccountFound()
    {
        return Problem(
            title: "Account not found",
            statusCode: StatusCodes.Status404NotFound
        );
    }
}