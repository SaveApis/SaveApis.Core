using SaveApis.Core.Common.Application.Hangfire.Types;
using SaveApis.Core.Common.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Common.Infrastructure.Hangfire.Events;

namespace Example.Web.Domains.Hangfire.Application.Hangfire.Events;

[HangfireRecurringEvent("test", "* * * * *", HangfireQueue.Low)]
public class TestRecurringEvent : IEvent;
