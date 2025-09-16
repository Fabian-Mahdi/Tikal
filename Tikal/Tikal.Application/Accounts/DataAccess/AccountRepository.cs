using Tikal.Domain.Accounts;

namespace Tikal.Application.Accounts.DataAccess;

public interface AccountRepository
{
    Task<Account> CreateAccount(Account account, CancellationToken cancellationToken);

    Task<Account?> GetAccountById(string id, CancellationToken cancellationToken);
}