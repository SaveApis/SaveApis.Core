using System.Reflection;
using SaveApis.Core.Web.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WithAssemblies(Assembly.GetExecutingAssembly()).WithSaveApis();

var app = builder.Build();

await app.RunSaveApisAsync().ConfigureAwait(false);
