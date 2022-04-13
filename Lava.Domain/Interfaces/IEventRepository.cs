using Lava.Domain.Aggregates;

namespace Lava.Domain.Interfaces;

public interface IEventRepository
{
    Task<TAggregator> Get<TAggregator>(string id) where TAggregator : AbstractAggregator, new();
    Task SaveChanges<TAggregator>(TAggregator aggregateRoot) where TAggregator : AbstractAggregator;
}