using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

// database
IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false);

// identity db
IResourceBuilder<PostgresDatabaseResource> identityDb = postgres.AddDatabase("identitydb");

// identity api
IResourceBuilder<ProjectResource> identityapi = builder.AddProject<IdentityAPI>("IdentityAPI")
    .WithReference(identityDb)
    .WaitFor(identityDb);

// tikal db
IResourceBuilder<PostgresDatabaseResource> tikaldb = postgres.AddDatabase("TikalDatabase");

// tikal backend
IResourceBuilder<ProjectResource> tikalApp = builder.AddProject<Tikal_App>("TikalApp")
    .WithReference(tikaldb)
    .WaitFor(tikaldb);

// frontend
builder.AddNpmApp("frontend", "../Frontend")
    .WithReference(identityapi)
    .WithReference(tikalApp)
    .WaitFor(identityapi)
    .WaitFor(tikalApp);

builder.Build().Run();