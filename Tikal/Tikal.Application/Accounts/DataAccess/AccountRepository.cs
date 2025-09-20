using Tikal.Domain.Accounts;

namespace Tikal.Application.Accounts.DataAccess;

/// <summary>
///     The repository for all operations regarding <see cref="Account" />
/// </summary>
public interface AccountRepository
{
    /// <summary>
    ///     Creates a given <see cref="Account" />
    /// </summary>
    /// <param name="account">The <see cref="Account" /> to create</param>
    /// <param name="cancellationToken">
    ///     <see cref="CancellationToken" />
    /// </param>
    /// <returns>The created <see cref="Account" /></returns>
    Task<Account> CreateAccount(Account account, CancellationToken cancellationToken);

    /// <summary>
    ///     Gets an <see cref="Account" /> for a given id
    /// </summary>
    /// <param name="id">The id of the <see cref="Account" /> to retrieve</param>
    /// <param name="cancellationToken">
    ///     <see cref="CancellationToken" />
    /// </param>
    /// <returns>
    ///     The <see cref="Account" /> with the given id, null if no <see cref="Account" /> with the given id could be
    ///     found
    /// </returns>
    Task<Account?> GetAccountById(string id, CancellationToken cancellationToken);
}