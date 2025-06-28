using IdentityAPI.Configuration;
using IdentityAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityAPI.Database;

public class ApplicationDbContext : IdentityDbContext<User>
{
    private readonly DatabaseOptions options;

    public ApplicationDbContext(IOptions<DatabaseOptions> options, DbContextOptions<ApplicationDbContext> contextOptions)
        : base(contextOptions)
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
}