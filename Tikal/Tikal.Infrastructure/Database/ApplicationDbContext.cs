using Microsoft.EntityFrameworkCore;
using Tikal.Infrastructure.Entities;

namespace Tikal.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<AccountEntity> Accounts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}