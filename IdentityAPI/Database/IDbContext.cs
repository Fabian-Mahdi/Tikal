using IdentityAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Database;

public interface IDbContext
{
    public DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    void Dispose();
}