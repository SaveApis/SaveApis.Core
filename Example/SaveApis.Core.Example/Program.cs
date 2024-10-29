using System.Reflection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Example.Application.Backend.GraphQL;
using SaveApis.Core.Example.Application.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WithAssemblies(Assembly.GetExecutingAssembly())
    .WithAutofac<ExampleQuery, ExampleMutation>((containerBuilder, configuration) =>
        containerBuilder.WithSignalR().WithMongo(configuration).WithEfCore(configuration));

builder.Services.AddControllers();

var app = builder.Build();

app.MapHub<TestHub>("/test").RequireAuthorization();

await app.RunSaveApisAsync();