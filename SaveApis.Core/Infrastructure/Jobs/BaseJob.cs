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
        var scopedLogger = logger.ForContext("job.name", GetType().FullName);

        switch (level)
        {
            case LogEventLevel.Verbose:
                scopedLogger.Verbose(exception, message, [.. args]);
                break;
            case LogEventLevel.Debug:
                scopedLogger.Debug(exception, message, [.. args]);
                break;
            case LogEventLevel.Information:
                scopedLogger.Information(exception, message, [.. args]);
                break;
            case LogEventLevel.Warning:
                scopedLogger.Warning(exception, message, [.. args]);
                break;
            case LogEventLevel.Error:
                scopedLogger.Error(exception, message, [.. args]);
                break;
            case LogEventLevel.Fatal:
                scopedLogger.Fatal(exception, message, [.. args]);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }
}