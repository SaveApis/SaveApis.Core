using MediatR;

namespace SaveApis.Core.Common.Infrastructure.Hangfire.Events;

/// <summary>
/// Interface to mark a JobEvent
/// </summary>
public interface IEvent : INotification;
