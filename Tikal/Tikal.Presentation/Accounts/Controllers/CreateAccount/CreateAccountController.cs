using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using Tikal.Application.Accounts.Commands.CreateAccount;
using Tikal.Domain.Accounts;
using Tikal.Presentation.Accounts.Dtos;
using Tikal.Presentation.Core;

namespace Tikal.Presentation.Accounts.Controllers.CreateAccount;

[Route("accounts")]
public partial class CreateAccountController : ApiController
{
    public CreateAccountController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto, CancellationToken cancellationToken)
    {
        CreateAccountCommand command = new(dto.Id, dto.Name);

        OneOf<Account, DuplicateAccountId> result = await sender.Send(command, cancellationToken);

        return result.Match(
            handleSuccess,
            handleDuplicateAccountId
        );
    }

    private IActionResult handleSuccess(Account account)
    {
        AccountDto dto = new(account.Id, account.Name);

        return Ok(dto);
    }

    private IActionResult handleDuplicateAccountId(DuplicateAccountId duplicateAccountId)
    {
        return AccountExists(duplicateAccountId.Id);
    }
}