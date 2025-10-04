using FluentValidation;
using Identity.App.Extensions;
using Identity.Application;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;

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

builder.Services.AddIdentity<UserEntity, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Dependencies
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDevMediatr();
}
else
{
    builder.Services.AddProdMediatr(builder.Configuration);
}

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddRepositories();

builder.Services.AddMappers();

builder.Services.AddPipelines();

builder.Services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

builder.Services.AddControllers().AddApplicationPart(Identity.Presentation.AssemblyReference.Assembly);

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

app.MapHealthChecks("/healthcheck");

app.MapControllers();

await app.RunAsync();