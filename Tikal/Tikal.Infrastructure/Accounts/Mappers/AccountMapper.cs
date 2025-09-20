using Tikal.Domain.Accounts;
using Tikal.Infrastructure.Entities;

namespace Tikal.Infrastructure.Accounts.Mappers;

/// <summary>
///     Used to map <see cref="Account" /> to <see cref="AccountEntity" /> and vice versa
/// </summary>
public class AccountMapper
{
    /// <summary>
    ///     Maps a given <see cref="Account" /> to an <see cref="AccountEntity" />
    /// </summary>
    /// <param name="account">The <see cref="Account" /> to map</param>
    /// <returns>The resulting <see cref="AccountEntity" /></returns>
    public AccountEntity FromAccount(Account account)
    {
        return new AccountEntity
        {
            Id = account.Id,
            Name = account.Name
        };
    }

    /// <summary>
    ///     Maps a given <see cref="AccountEntity" /> to an <see cref="Account" />
    /// </summary>
    /// <param name="accountEntity">The <see cref="AccountEntity" /> to map</param>
    /// <returns>The resulting <see cref="Account" /></returns>
    public Account ToAccount(AccountEntity accountEntity)
    {
        return new Account(accountEntity.Id, accountEntity.Name);
    }
}