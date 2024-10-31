using System.Reflection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Example.Application.Backend.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WithAssemblies(Assembly.GetExecutingAssembly())
    .WithAutofac((containerBuilder, configuration) => containerBuilder.WithMongo(configuration)
        .WithSwagger(configuration).WithGraphQl<ExampleQuery, ExampleMutation>(configuration)
        .WithAutoMapper(configuration).WithFluentValidator(configuration)
        .WithSignalR(configuration));

builder.Services.AddControllers();

var app = builder.Build();

await app.RunSaveApisAsync();