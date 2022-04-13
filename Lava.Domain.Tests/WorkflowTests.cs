using Lava.Domain.Aggregates;
using Lava.Domain.Events;
using FluentAssertions;
using Lava.Domain.Interfaces;
using NUnit.Framework;

namespace Lava.Domain.Tests;

public sealed class WorkflowTests : AbstractAggregatorTests<Workflow>
{
    private readonly string _workflowId = "workflow-id";
    private readonly string _createdBy = "created-by";
    private readonly DateTime _createdAt = new DateTime(2000, 1, 1);
    private readonly string _workflowCreatedType = "workflow-created";

    [SetUp]
    public void SetUp()
    {
        TestSubject = new Workflow();
    }
    
    [Test]
    public void Create_Generates_WorkflowCreated()
    {
        Given();

        When(command => command.Create(_workflowId, _createdBy, _createdAt));

        Then<WorkflowCreated>(
            ev => ev.StreamId.Should().Be(_workflowId),
            ev => ev.WorkflowId.Should().Be(_workflowId),
            ev => ev.CreatedBy.Should().Be(_createdBy),
            ev => ev.CreatedAt.Should().Be(_createdAt),
            ev => ev.Type.Should().Be(_workflowCreatedType)
            );
    }
    
    [Test]
    public void Create_ThrowsException_WhenWorkflowAlreadyExists()
    {
        Given(GetWorkflowCreated());

        Throws<InvalidOperationException>(command => command.Create(_workflowId, _createdBy, _createdAt), 
            ex =>
                ex.Message.Should().Satisfy(s => s.Be("Workflow with id already exists")) &&
                ex.InnerException.Should().Satisfy(s => s.BeNull()));
    }

    private IEvent GetWorkflowCreated()
    {
        return new WorkflowCreated(_workflowId, _createdBy, _createdAt);
    }
}