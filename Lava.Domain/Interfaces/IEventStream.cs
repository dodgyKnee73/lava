namespace Lava.Domain.Interfaces;

public interface IEventStream
{
    IAsyncEnumerable<IEvent> GetStream(string streamId);
    Task Append<TEventRecord>(string streamId, string type, TEventRecord eventRecord) where TEventRecord : IEvent;
}