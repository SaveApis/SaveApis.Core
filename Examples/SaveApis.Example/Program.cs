using System.Reflection;
using SaveApis.Core.Application.Jwt;
using SaveApis.Core.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// .WithAssemblies is required to register custom services created in THIS project
// .AddTypes is required to register Queries/Mutations
builder
    .WithAssemblies(Assembly.GetExecutingAssembly())
    .AddSaveApis(executorBuilder => executorBuilder.AddTypes(), AuthenticationMode.Jwt);

var app = builder.Build();

await app.RunSaveApisAsync(args).ConfigureAwait(false);
