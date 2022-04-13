using Lava.Domain.Interfaces;

namespace Lava.Domain.Adapters;

public class InMemory : IEventStream
{
    private readonly Dictionary<string, Queue<IEvent>> _eventStreams;

    public InMemory()
    {
        _eventStreams = new Dictionary<string, Queue<IEvent>>();
    }

    public Task Append<T>(string streamId, string type, T eventRecord) where T : IEvent
    {
        if (!_eventStreams.ContainsKey(streamId))
        {
            _eventStreams.Add(streamId, new Queue<IEvent>());
        }
        _eventStreams[streamId].Enqueue(eventRecord);
        return Task.CompletedTask;
    }

    public IAsyncEnumerable<IEvent> GetStream(string streamId)
    {
        return _eventStreams.ContainsKey(streamId) 
            ? _eventStreams[streamId].ToAsyncEnumerable() 
            : Enumerable.Empty<IEvent>().ToAsyncEnumerable();
    }
}