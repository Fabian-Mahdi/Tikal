using Microsoft.AspNetCore.HttpLogging;
using Tikal.App.Extensions;
using Tikal.Application;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

// Database
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDevDbContext(builder);
}

// Dependencies
builder.Services.AddRepositories();

builder.Services.AddMappers();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly));

builder.Services.AddControllers().AddApplicationPart(Tikal.Presentation.AssemblyReference.Assembly);

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseHttpLogging();

app.MapHealthChecks("/healthcheck");

app.MapControllers();

app.Run();