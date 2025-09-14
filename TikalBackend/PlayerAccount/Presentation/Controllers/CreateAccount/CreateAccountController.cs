using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using Shared.Bases.Presentation;
using TikalBackend.PlayerAccount.Domain.Commands.CreateAccount;
using TikalBackend.PlayerAccount.Domain.Models;
using TikalBackend.PlayerAccount.Presentation.Dtos;

namespace TikalBackend.PlayerAccount.Presentation.Controllers.CreateAccount;

[ApiController]
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
        AccountDto dto = new(account.Id, account.Username);

        return Ok(dto);
    }

    private IActionResult handleDuplicateAccountId(DuplicateAccountId duplicateAccountId)
    {
        return AccountExists(duplicateAccountId.Id);
    }
}