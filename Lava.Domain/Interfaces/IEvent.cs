namespace Lava.Domain.Interfaces;

public interface IEvent
{
    string StreamId { get; }
    string Type { get; }
}
