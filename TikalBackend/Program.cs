using Microsoft.AspNetCore.HttpLogging;
using Shared.Extensions.LoggingBuilder;
using Shared.Extensions.ServiceCollection;
using Shared.Extensions.WebApp;
using TikalBackend.Database;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.ConfigureOpenTelemetry("TikalBackend");

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDevDbContext<ApplicationDbContext>(builder, "TikalDatabase");
    builder.Services.AddDevOpenTelemetry();
}

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = false;
});

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ApplicationDbContext).Assembly));

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations<ApplicationDbContext>();
}

app.UseHttpLogging();

app.MapHealthChecks("/healthcheck");

app.MapControllers();

app.Run();