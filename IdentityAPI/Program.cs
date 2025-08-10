using IdentityAPI.Authentication.Infrastructure.Entities;
using IdentityAPI.Database;
using IdentityAPI.Extensions;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

if (builder.Environment.IsDevelopment())
{
    builder.Logging.ConfigureDevOpenTelemetry(builder.Configuration);
    builder.Services.AddDevCorsPolicy();
}
else
{
    builder.Configuration.ConfigureKeyVault();
    builder.Services.AddAzureOpenTelemetry(builder.Configuration);
}

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddExceptionHandler();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddAuthenticationDependencyGroup();

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("development");
    app.ApplyMigrations();
}

// seed identity data
using (IServiceScope scope = app.Services.CreateScope())
{
    await IdentitySeedData.InitializeAsync(scope.ServiceProvider);
}

app.UseHttpLogging();

app.UseExceptionHandler();

app.MapHealthChecks("/healthcheck");

app.MapControllers();

await app.RunAsync();