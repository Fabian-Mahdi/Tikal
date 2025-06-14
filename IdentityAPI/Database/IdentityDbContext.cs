using IdentityAPI.Configuration;
using IdentityAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityAPI.Database;

public class IdentityDbContext : DbContext
{
    private readonly DatabaseOptions options;

    public IdentityDbContext(IOptions<DatabaseOptions> options, DbContextOptions<IdentityDbContext> contextOptions) : base(contextOptions)
    {
        this.options = options.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            $"Server={options.Host};" +
            $"Port={options.Port};" +
            $"Database={options.DatabaseName};" +
            $"User ID={options.Username};" +
            $"Password={options.Password};"
            );
    }

    public DbSet<User> Users { get; set; }
}