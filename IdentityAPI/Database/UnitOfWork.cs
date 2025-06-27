using IdentityAPI.Database.Exceptions;
using IdentityAPI.Database.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace IdentityAPI.Database;

public class UnitOfWork : IDisposable
{
    private readonly IdentityDbContext context;

    public IUserRepository UserRepository { get; private set; }

    public UnitOfWork(IdentityDbContext context, IUserRepository userRepository)
    {
        this.context = context;
        UserRepository = userRepository;
    }

    public async Task Commit()
    {
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is not PostgresException postgresException)
            {
                throw;
            }

            switch (postgresException.SqlState)
            {
                // for error codes see: https://www.postgresql.org/docs/current/errcodes-appendix.html
                case "23505":
                    throw new UniqueConstraintViolationException();
            }
        }
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}