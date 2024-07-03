using SaveApis.Core.Application.Jobs.Hangfire;
using SaveApis.Core.Infrastructure.Events;

namespace SaveApis.Core.Example.Application.Events.Recurring;

/// <summary>
/// Remove this class to test <see cref="CleanupRecurringEventsJob" />
/// </summary>
public class TestRecurringEvent() : BaseRecurringEvent("test", "* * * * *");