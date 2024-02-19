var builder = DistributedApplication.CreateBuilder(args);

var backend = builder.AddProject<Projects.Backend_Web>("backend");

builder.AddProject<Projects.AspireJavaScript_MinimalApi>("weatherapi");

builder.AddNpmApp("frontend", "../../frontend", "dev")
    .WithReference(backend)
    .WithServiceBinding(containerPort: 3002, scheme: "http", env: "PORT")
    .AsDockerfileInManifest();

var app = builder.Build();

app.Run();
