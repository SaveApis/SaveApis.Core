using Hangfire;
using SaveApis.Core.Application.Events;
using SaveApis.Core.Infrastructure.Jobs;
using SaveApis.Core.Infrastructure.Persistence.Mongo.Interfaces;
using Serilog;
using Serilog.Events;

namespace SaveApis.Core.Application.Jobs.Mongo;

[Queue("system")]
public class GenerateMongoIndexesJob(ILogger logger, IEnumerable<IMongoIndex> indices)
    : BaseModuleSpecifiedJob<ApplicationStartedEvent>(logger)
{
    [JobDisplayName("Generate mongo indexes")]
    public override async Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        var indicesList = indices.ToList();
        Log(LogEventLevel.Information, "Generate {Count} indexes.", null, indicesList.Count);
        foreach (var index in indicesList)
        {
            Log(LogEventLevel.Information, "Create index {Index}.", null, index.Name);
            await index.Create();
        }
    }
}