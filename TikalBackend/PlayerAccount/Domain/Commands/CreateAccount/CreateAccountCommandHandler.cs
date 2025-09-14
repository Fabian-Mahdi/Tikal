using OneOf;
using Shared.Abstractions.Messaging;
using TikalBackend.PlayerAccount.Domain.Models;

namespace TikalBackend.PlayerAccount.Domain.Commands.CreateAccount;

public class CreateAccountCommandHandler : CommandHandler<CreateAccountCommand, OneOf<Account, DuplicateAccountId>>
{
    public async Task<OneOf<Account, DuplicateAccountId>> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        return new Account(request.id, request.name);
    }
}