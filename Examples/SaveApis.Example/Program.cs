using SaveApis.Core.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// .AddTypes is required to register Queries/Mutations
builder.AddSaveApis(executorBuilder => executorBuilder.AddTypes());

var app = builder.Build();

await app.RunSaveApisAsync(args).ConfigureAwait(false);
