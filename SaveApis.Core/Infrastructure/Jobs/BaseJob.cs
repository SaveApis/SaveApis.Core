using Hangfire;
using SaveApis.Core.Infrastructure.Events.Interfaces;
using SaveApis.Core.Infrastructure.Jobs.Interfaces;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace SaveApis.Core.Infrastructure.Jobs;

[Queue("medium")]
public abstract class BaseJob<TEvent>(ILogger logger) : IJob<TEvent>
    where TEvent : IEvent
{
    public abstract Task RunAsync(TEvent @event, CancellationToken cancellationToken = default);

    [MessageTemplateFormatMethod("message")]
    protected void Log(LogEventLevel level, string message, Exception? exception = null, params object[] args)
    {
        logger.Write(level, exception, $"{{JobName}} | {message}", [GetType().Name, .. args]);
    }
}