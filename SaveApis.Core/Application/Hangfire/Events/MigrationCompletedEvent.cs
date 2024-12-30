using SaveApis.Core.Infrastructure.Hangfire.Events;

namespace SaveApis.Core.Application.Hangfire.Events;

public record MigrationCompletedEvent : IEvent;
