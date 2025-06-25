using IdentityAPI.Database;
using IdentityAPI.Extensions;
using Microsoft.AspNetCore.HttpLogging;

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

builder.Services.AddDbContext<IdentityDbContext>();

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.AddAuthenticationDependencyGroup();

builder.Services.AddExceptionHandler();

builder.Services.AddHealthChecks();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpLogging();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapHealthChecks("/healthcheck");

app.MapControllers();

app.Run();