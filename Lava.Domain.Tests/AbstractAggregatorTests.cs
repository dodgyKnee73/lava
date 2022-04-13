using Lava.Domain.Interfaces;
using Lava.Domain.Aggregates;
using FluentAssertions;

namespace Lava.Domain.Tests;

public abstract class AbstractAggregatorTests<TAggregate> where TAggregate : AbstractAggregator
{
    protected TAggregate TestSubject = null!;

    protected void Given(params IEvent[] events)
    {
        foreach (var ev in events)
        {
            TestSubject.When(ev);
        }
    }

    protected void When(Action<TAggregate> command)
    {
        command(TestSubject as TAggregate);
    }

    protected void Then<TEvent>(params Action<TEvent>[] eventExpectations) where TEvent : IEvent
    {
        var scopedTestSubject = TestSubject as TAggregate;
        var actual = scopedTestSubject.GetPending();

        // assert - global test expectations
        actual.Should().BeOfType<TEvent>();

        // assert - test specific expectations
        foreach (var expectation in eventExpectations)
        {
            expectation((TEvent) actual);
        }
    }

    protected void Throws<TException>(Action<TAggregate> command, Func<TException, bool> IsMatch)
        where TException : Exception
    {
        TestSubject.Invoking(cmd => command(TestSubject as TAggregate))
            .Should().Throw<TException>()
            .And
            .Should().Match<TException>(x => IsMatch(x));
    }
}