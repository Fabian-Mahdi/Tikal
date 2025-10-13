using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tikal.Application.Accounts.Queries.GetAccount;
using Tikal.Domain.Accounts;
using Tikal.Presentation.Accounts.Dtos;
using Tikal.Presentation.Core;

namespace Tikal.Presentation.Accounts.Controllers.GetAccount;

[Route("accounts")]
public partial class GetAccountController : ApiController
{
    public GetAccountController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAccount(CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        GetAccountQuery query = new(userId);

        Account? account = await sender.Send(query, cancellationToken);

        return account is null ? NoAccountFound() : handleSuccess(account);
    }

    private OkObjectResult handleSuccess(Account account)
    {
        AccountDto dto = new(account.Name);

        return Ok(dto);
    }
}