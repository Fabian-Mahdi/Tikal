using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

// database
IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false);

// identity db
IResourceBuilder<PostgresDatabaseResource> identitydb = postgres.AddDatabase("IdentityDatabase");

// identity
IResourceBuilder<ProjectResource> identity = builder.AddProject<Identity_App>("Identity")
    .WithReference(identitydb)
    .WaitFor(identitydb);

// tikal db
IResourceBuilder<PostgresDatabaseResource> tikaldb = postgres.AddDatabase("TikalDatabase");

// tikal backend
IResourceBuilder<ProjectResource> tikalApp = builder.AddProject<Tikal_App>("TikalApp")
    .WithReference(tikaldb)
    .WaitFor(tikaldb);

// frontend
builder.AddNpmApp("frontend", "../Frontend")
    .WithReference(identity)
    .WithReference(tikalApp)
    .WaitFor(identity)
    .WaitFor(tikalApp);

builder.Build().Run();