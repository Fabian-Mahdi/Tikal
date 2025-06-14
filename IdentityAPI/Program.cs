using IdentityAPI.Database;
using IdentityAPI.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddDbContext<IdentityDbContext>();

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.AddAuthenticationDependencyGroup();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();