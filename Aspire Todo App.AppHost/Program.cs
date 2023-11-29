using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var todoapi = builder.AddProject<Projects.AspireTodoApp_Api>("aspiretodoapp.api");

builder.AddNpmApp("frontend", "../todo-app")
    .WithReference(todoapi)
    .WithServiceBinding(scheme: "http", hostPort: 4200);


builder.Build().Run();


