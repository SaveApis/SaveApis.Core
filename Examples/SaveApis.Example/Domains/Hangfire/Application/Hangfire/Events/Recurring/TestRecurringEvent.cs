using SaveApis.Core.Application.Hangfire;
using SaveApis.Core.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Infrastructure.Hangfire.Events;

namespace SaveApis.Example.Domains.Hangfire.Application.Hangfire.Events.Recurring;

[HangfireRecurringEvent("test", "*/5 * * * *", HangfireQueue.Low)]
public class TestRecurringEvent : IEvent;
