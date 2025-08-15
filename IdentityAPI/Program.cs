using IdentityAPI.Database;
using IdentityAPI.Extensions;
using IdentityAPI.Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Sentry.OpenTelemetry;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.ConfigureOpenTelemetry();

builder.Services.AddConfiguration(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDevDbContext(builder);
    builder.Services.AddDevOpenTelemetry();
}
else
{
    builder.Configuration.ConfigureKeyVault();
    builder.WebHost.UseSentry(options => { options.UseOpenTelemetry(); });
    builder.Services.AddProdDbContext(builder.Configuration);
    builder.Services.AddProdOpenTelemetry();
    builder.Services.AddProdCorsPolicy();
}

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});

builder.Services.AddCustomProblemDetails();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthenticationDependencyGroup();

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}
else
{
    app.UseCors("production");
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