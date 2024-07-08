using System.Reflection;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Example.Application.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WithAssemblies(Assembly.GetExecutingAssembly())
    .WithAutofac();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers().RequireAuthorization();
app.MapHub<TestHub>("/test").RequireAuthorization();

await app.RunSaveApisAsync();