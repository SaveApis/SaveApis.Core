using Hangfire;
using MediatR;
using SaveApis.Core.Application.Commands.PushCacheObject;
using SaveApis.Core.Application.Events;
using SaveApis.Core.Application.Extensions;
using SaveApis.Core.Application.Models.ValueObject;
using SaveApis.Core.Infrastructure.Jobs;
using ILogger = Serilog.ILogger;

namespace SaveApis.Core.Example.Application.Jobs;

[Queue("low")]
public class TestEasyCachingJob(ILogger logger, IMediator mediator) : BaseJob<ApplicationStartedEvent>(logger)
{
    [JobDisplayName("EasyCaching")]
    public override async Task RunAsync(ApplicationStartedEvent @event, CancellationToken cancellationToken = default)
    {
        var key = CacheKey.Create(CacheName.Create("test"), "test");
        var result =
            await mediator.Send(new PushCacheObjectCommand(key, "test", TimeSpan.FromHours(1)),
                cancellationToken);
        result.ThrowOnErrors();
    }
}