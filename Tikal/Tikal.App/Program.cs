using Microsoft.AspNetCore.HttpLogging;
using Tikal.App.Extensions;
using Tikal.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuration
if (builder.Environment.IsProduction())
{
    builder.Configuration.ConfigureKeyVault();
    builder.Services.AddProductionCorsPolicy();
}

// Logging
builder.Logging.ClearProviders();
builder.Logging.ConfigureOpenTelemetry();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDevOpenTelemetry();
}
else
{
    builder.WebHost.AddSentry();
    builder.Services.AddProdOpenTelemetry();
}

// Database
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDevDbContext(builder);
}
else
{
    builder.Services.AddProdDbContext(builder.Configuration);
}

// Auth
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMandatoryAuthorization();

// Dependencies
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDevMediatr();
}
else
{
    builder.Services.AddProdMediatr(builder.Configuration);
}

builder.Services.AddRepositories();

builder.Services.AddMappers();

builder.Services.AddPipelines();

builder.Services.AddControllers().AddApplicationPart(AssemblyReference.Assembly);

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

app.UseHttpLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("/healthcheck").AllowAnonymous();

app.MapControllers();

await app.RunAsync();