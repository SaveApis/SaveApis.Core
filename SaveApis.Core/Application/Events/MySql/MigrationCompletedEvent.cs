using SaveApis.Core.Infrastructure.Events.Interfaces;

namespace SaveApis.Core.Application.Events.MySql;

public record MigrationCompletedEvent : IEvent;