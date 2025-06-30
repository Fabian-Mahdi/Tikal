using IdentityAPI.Database;
using IdentityAPI.Extensions;
using IdentityAPI.Models;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

if (builder.Environment.IsDevelopment())
{
    builder.Logging.ConfigureDevOpenTelemetry(builder.Configuration);
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

builder.Services.AddJwtDependencyGroup();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler();

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

// seed identity data
using (var scope = app.Services.CreateScope())
{
    await IdentitySeedData.InitializeAsync(scope.ServiceProvider);
}

app.UseHttpLogging();

app.UseExceptionHandler();

app.MapHealthChecks("/healthcheck");

app.MapControllers();

app.Run();