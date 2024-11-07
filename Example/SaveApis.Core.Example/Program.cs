using System.Reflection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Example.Application.Backend.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WithAutofac((containerBuilder, configuration) => containerBuilder
        .WithAssemblies(Assembly.GetExecutingAssembly()).WithMongo(configuration)
        .WithEfCore(configuration).WithSwagger(configuration).WithGraphQl<ExampleQuery, ExampleMutation>(configuration)
        .WithAutoMapper(configuration).WithFluentValidator(configuration).WithSignalR(configuration));

builder.Services.AddControllers();

var app = builder.Build();

await app.RunSaveApisAsync();