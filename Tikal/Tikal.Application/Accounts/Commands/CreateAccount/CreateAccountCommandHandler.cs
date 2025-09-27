using OneOf;
using OneOf.Types;
using Tikal.Application.Accounts.DataAccess;
using Tikal.Application.Core.DataAccess;
using Tikal.Application.Core.Errors;
using Tikal.Application.Core.Messaging;
using Tikal.Domain.Accounts;

namespace Tikal.Application.Accounts.Commands.CreateAccount;

/// <summary>
///     The command handler for <see cref="CreateAccountCommand" />
/// </summary>
public sealed class CreateAccountCommandHandler
    : CommandHandler<CreateAccountCommand, OneOf<Account, ValidationFailed, DuplicateAccountId>>
{
    private readonly AccountRepository accountRepository;

    private readonly UnitOfWork unitOfWork;

    public CreateAccountCommandHandler(AccountRepository accountRepository, UnitOfWork unitOfWork)
    {
        this.accountRepository = accountRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<OneOf<Account, ValidationFailed, DuplicateAccountId>> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        Account? existingAccount = await accountRepository.GetAccountById(request.id, cancellationToken);

        if (existingAccount is not null)
        {
            return new DuplicateAccountId(request.id);
        }

        Account account = new(request.id, request.name);

        Account createdAccount = await accountRepository.CreateAccount(account, cancellationToken);

        OneOf<None, DatabaseUpdateError> result = await unitOfWork.SaveChanges(cancellationToken);

        return result.Match<OneOf<Account, ValidationFailed, DuplicateAccountId>>(
            _ => createdAccount,
            _ => new DuplicateAccountId(request.id)
        );
    }
}