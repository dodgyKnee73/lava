using FluentAssertions;
using Lava.Domain.Interfaces;
using Lava.Domain.Repository;
using NSubstitute;
using NUnit.Framework;

namespace Lava.Domain.Tests;

public class EventRepositoryTests
{
    private IEventRepository _testSubject = null!;
    private IEventStream _mockEventStream = null!;
    private readonly string id = "event-stream-id";

    [SetUp]
    public void Setup()
    {
        _mockEventStream = Substitute.For<IEventStream>();
        _testSubject = new EventRepository(
            _mockEventStream
            );
    }
    
    [Test]
    public async Task Get_Returns_AggregateRoot()
    {
        // arrange 
        var eventStream = new List<IEvent> 
        {
            new TestEvent(id, "test-event-1") 
        };

        _mockEventStream.GetStream(id).Returns(eventStream.ToAsyncEnumerable());

        // act
        var actual = await _testSubject.Get<TestAggregateRoot>(id);

        // assert
        actual.Message.Should().Be("test-event-1");
    }

    [Test]
    public async Task Get_AppliesAllEventsTo_AggregateRoot()
    {
        // arrange 
        var eventStream = new List<IEvent>
        {
            new TestEvent(id, "test-event-1"),
            new TestEvent(id, "test-event-2")
        };
    
        _mockEventStream.GetStream(id).Returns(eventStream.ToAsyncEnumerable());
    
        // act
        var actual = await _testSubject.Get<TestAggregateRoot>(id);
    
        // assert
        actual.Message.Should().Be("test-event-2");
    }
    
    [Test]
    public async Task SaveChanges_ForAggregateRoot_AppendsEventsToStream()
    {
        // arrange 
        var expected = new TestEvent(id, "test-event-1");
        var testAggregateRoot = new TestAggregateRoot();
        testAggregateRoot.NewTest(expected.Id, expected.Message);
    
        // act
        await _testSubject.SaveChanges(testAggregateRoot);
    
        // assert
        await _mockEventStream.Received(1).Append(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEvent>());
        await _mockEventStream.Received(1).Append(id, "test-event", Arg.Is<IEvent>(arg => IsValidEvent(arg, expected)));
    }
        
    [Test]
    public async Task SaveChanges_ForAggregateRoot_SavesLastEvent()
    {
        // arrange 
        var expected = new TestEvent(id, "test-event-2");
        var testAggregateRoot = new TestAggregateRoot();
        testAggregateRoot.When(new TestEvent(id, "test-event-1"));
        testAggregateRoot.NewTest(expected.Id, expected.Message);
    
        // act
        await _testSubject.SaveChanges(testAggregateRoot);
    
        // assert
        await _mockEventStream.Received(1).Append(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEvent>());
        await _mockEventStream.Received(1).Append(id, "test-event", Arg.Is<IEvent>(arg => IsValidEvent(arg, expected)));
    }
    
    private static bool IsValidEvent(IEvent actual, IEvent expected)
    {
        return actual.Should().Satisfy(should => should.BeEquivalentTo(expected));
    }
}