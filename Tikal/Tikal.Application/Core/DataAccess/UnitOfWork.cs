using OneOf;
using OneOf.Types;

namespace Tikal.Application.Core.DataAccess;

/// <summary>
///     Encompasses all database changes of the current scope
/// </summary>
public interface UnitOfWork
{
    /// <summary>
    ///     Commits all database changes of the curent scope
    /// </summary>
    /// <returns><see cref="None" /> if the operation was successful, otherwise <see cref="DatabaseUpdateError" /></returns>
    Task<OneOf<None, DatabaseUpdateError>> SaveChanges(CancellationToken cancellationToken);
}