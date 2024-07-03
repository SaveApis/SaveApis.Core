namespace SaveApis.Core.Infrastructure.Events.Interfaces;

public interface IRecurringEvent : IEvent
{
    string Id { get; }
    string CronExpression { get; }
}