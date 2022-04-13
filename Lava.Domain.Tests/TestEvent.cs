using Lava.Domain.Events;

namespace Lava.Domain.Tests;

public record TestEvent(string Id, string Message) : AbstractEvent("test-event")
{
    public override string StreamId => Id;
}