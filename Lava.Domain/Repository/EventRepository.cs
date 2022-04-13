using Lava.Domain.Aggregates;
using Lava.Domain.Interfaces;

namespace Lava.Domain.Repository;

public sealed class EventRepository : IEventRepository
{
    private readonly IEventStream _eventStream;
    
    public EventRepository(IEventStream eventStream)
    {
        _eventStream = eventStream;
    }

    public async Task<T> Get<T>(string id) where T : AbstractAggregator, new()
    {
        var aggregateRoot = new T();
        await foreach (var ev in _eventStream.GetStream(id))
        {
            aggregateRoot.When(ev);
        }
        return aggregateRoot;
    }

    public async Task SaveChanges<T>(T aggregateRoot) where T : AbstractAggregator
    {
        var pendingEvent = aggregateRoot.GetPending();
        await _eventStream.Append(pendingEvent.StreamId, pendingEvent.Type, pendingEvent);
    }
}