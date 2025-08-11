using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

// database
IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false);

// identity db
IResourceBuilder<PostgresDatabaseResource> postgresdb = postgres.AddDatabase("identitydb");

// identity api
IResourceBuilder<ProjectResource> identityapi = builder.AddProject<IdentityAPI>("IdentityAPI")
    .WithReference(postgresdb);

// frontend
builder.AddNpmApp("frontend", "../Frontend")
    .WithReference(identityapi);

builder.Build().Run();