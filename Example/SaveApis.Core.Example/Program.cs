using System.Reflection;
using SaveApis.Core.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WithAssemblies(Assembly.GetExecutingAssembly())
    .WithAutofac();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

await app.RunSaveApisAsync();