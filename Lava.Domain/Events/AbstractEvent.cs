using Lava.Domain.Interfaces;

namespace Lava.Domain.Events;

public abstract record AbstractEvent(string Type) : IEvent
{
    public abstract string StreamId { get; }
}