using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tikal.Infrastructure.Database;
using Tikal.Integration.Extensions;

namespace Tikal.Integration.Core;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string databaseConnectionString;

    public CustomWebApplicationFactory(string databaseConnectionString)
    {
        this.databaseConnectionString = databaseConnectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(TestAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.SchemeName,
                    _ => { }
                );

            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            });
        });

        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? databaseContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (databaseContextDescriptor != null)
            {
                services.Remove(databaseContextDescriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options => { options.UseNpgsql(databaseConnectionString); });

            // we want to recreate the database for every test
            rebuildDatabase(services);
        });
    }

    private static void rebuildDatabase(IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();

        ApplicationDbContext databaseContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        databaseContext.Database.DropTables();
        databaseContext.Database.Migrate();
    }
}