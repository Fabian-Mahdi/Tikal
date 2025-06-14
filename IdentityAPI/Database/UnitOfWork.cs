using IdentityAPI.Database.Repositories.UserRepository;

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