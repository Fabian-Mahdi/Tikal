using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using Tikal.Application.Core.DataAccess;

namespace Tikal.Infrastructure.Database;

public class ApplicationUnitOfWork : UnitOfWork
{
    private readonly ApplicationDbContext dbContext;

    public ApplicationUnitOfWork(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<OneOf<None, DatabaseUpdateError>> SaveChanges(CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            return new DatabaseUpdateError();
        }

        return new None();
    }
}