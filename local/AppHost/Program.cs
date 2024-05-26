using Projects;

var builder = DistributedApplication.CreateBuilder(args);
var backend = builder.AddProject<Backend_Web>("backend");

builder.AddNpmApp("frontend", "../../frontend", "dev")
    .WithReference(backend)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

var app = builder.Build();

app.Run();
