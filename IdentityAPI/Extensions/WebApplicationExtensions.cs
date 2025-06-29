using IdentityAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
    }
}