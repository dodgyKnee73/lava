using Lava.Domain.Aggregates;
using Lava.Domain.Interfaces;

namespace Lava.Domain.Tests;

public class TestAggregateRoot : AbstractAggregator
{
    public string Message { get; private set; } = null!;

    public void NewTest(string id, string message)
    {
        var newTestEvent = new TestEvent(id, message);
        Apply(newTestEvent);
        Add(newTestEvent);
    }

    public override void When(IEvent ev)
    {
        switch (ev)
        {
            case TestEvent testEvent:
                Apply(testEvent);
                Add(testEvent);
                break;
        }
    }

    private void Apply(TestEvent testEvent)
    {
        //AggregateRootId = testEvent.Id;
        Message = testEvent.Message;
    }
}
