using SaveApis.Core.Infrastructure.Events.Interfaces;

namespace SaveApis.Core.Infrastructure.Jobs.Interfaces;

public interface IModuleSpecifiedJob<in TEvent> : IJob<TEvent> where TEvent : IEvent;