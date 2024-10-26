using Hangfire;
using MediatR;
using SaveApis.Core.Application.Events;
using SaveApis.Core.Example.Application.Queries;
using SaveApis.Core.Infrastructure.Jobs;
using ILogger = Serilog.ILogger;

namespace SaveApis.Core.Example.Application.Jobs;

[Queue("low")]
public class TestEasyCachingJob(ILogger logger, IMediator mediator) : BaseJob<ApplicationStartedEvent>(logger)
{
    [JobDisplayName("EasyCaching")]
    public override async Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new TestCachedQuery(), cancellationToken);
        if (result.IsFailed)
        {
            throw new Exception("Failed to execute TestCachedQuery");
        }
    }
}