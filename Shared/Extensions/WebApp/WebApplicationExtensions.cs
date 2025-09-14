using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions.WebApp;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyMigrations<TContext>(this WebApplication app)
        where TContext : DbContext
    {
        using IServiceScope scope = app.Services.CreateScope();

        TContext context = scope.ServiceProvider.GetRequiredService<TContext>();

        context.Database.Migrate();

        return app;
    }
}