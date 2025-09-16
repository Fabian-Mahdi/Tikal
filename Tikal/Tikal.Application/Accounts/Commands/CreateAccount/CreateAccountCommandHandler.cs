using OneOf;
using OneOf.Types;
using Tikal.Application.Abstractions.Messaging;
using Tikal.Application.Accounts.DataAccess;
using Tikal.Application.Core.DataAccess;
using Tikal.Domain.Accounts;

namespace Tikal.Application.Accounts.Commands.CreateAccount;

public sealed class CreateAccountCommandHandler
    : CommandHandler<CreateAccountCommand, OneOf<Account, DuplicateAccountId>>
{
    private readonly AccountRepository accountRepository;

    private readonly UnitOfWork unitOfWork;

    public CreateAccountCommandHandler(AccountRepository accountRepository, UnitOfWork unitOfWork)
    {
        this.accountRepository = accountRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<OneOf<Account, DuplicateAccountId>> Handle(
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

        return result.Match<OneOf<Account, DuplicateAccountId>>(
            _ => createdAccount,
            _ => new DuplicateAccountId(request.id)
        );
    }
}