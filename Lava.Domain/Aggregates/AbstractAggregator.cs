using Lava.Domain.Interfaces;

namespace Lava.Domain.Aggregates;

public abstract class AbstractAggregator
{
    private readonly List<IEvent> _events = new();

    public abstract void When(IEvent ev);

    protected void Add(IEvent ev)
    {
        _events.Add(ev);
    }

    public IEvent GetPending()
    {
        return _events.Last();
    }
}