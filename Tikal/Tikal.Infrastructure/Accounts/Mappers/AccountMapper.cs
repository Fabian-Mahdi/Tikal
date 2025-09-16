using Tikal.Domain.Accounts;
using Tikal.Infrastructure.Entities;

namespace Tikal.Infrastructure.Accounts.Mappers;

public class AccountMapper
{
    public AccountEntity FromAccount(Account account)
    {
        return new AccountEntity
        {
            Id = account.Id,
            Name = account.Name
        };
    }

    public Account ToAccount(AccountEntity accountEntity)
    {
        return new Account(accountEntity.Id, accountEntity.Name);
    }
}