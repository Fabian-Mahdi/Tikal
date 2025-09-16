using OneOf;
using OneOf.Types;

namespace Tikal.Application.Core.DataAccess;

public interface UnitOfWork
{
    Task<OneOf<None, DatabaseUpdateError>> SaveChanges(CancellationToken cancellationToken);
}