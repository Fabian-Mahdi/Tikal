using IdentityAPI.Database;
using IdentityAPI.Integration.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Integration.Base;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string connectionString;

    public CustomWebApplicationFactory(string connectionString)
    {
        this.connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
            );

            if (dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            // we want to recreate the db for every test
            using IServiceScope scope = services.BuildServiceProvider().CreateScope();
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.DropTables();
            context.Database.Migrate();
        });
    }
}