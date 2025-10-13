using Tikal.Application.Accounts.DataAccess;
using Tikal.Application.Core.Messaging;
using Tikal.Domain.Accounts;

namespace Tikal.Application.Accounts.Queries.GetAccount;

/// <summary>
///     The query handler for <see cref="GetAccountQuery" />
/// </summary>
public class GetAccountQueryHandler
    : QueryHandler<GetAccountQuery, Account?>
{
    private readonly AccountRepository accountRepository;

    public GetAccountQueryHandler(AccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    public async Task<Account?> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        Account? account = await accountRepository.GetAccountById(request.id, cancellationToken);

        return account;
    }
}