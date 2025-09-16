using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tikal.Presentation.Accounts.Controllers.CreateAccount;

public partial class CreateAccountController
{
    private ObjectResult AccountExists(string id)
    {
        return Problem(
            title: "Account already exists",
            detail: $"An account with the id '{id}' already exists.",
            statusCode: StatusCodes.Status409Conflict
        );
    }
}