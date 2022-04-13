using Lava.Domain.Interfaces;
using Lava.Domain.Events;

namespace Lava.Domain.Aggregates;

public sealed class Workflow : AbstractAggregator
{
    private string WorkflowId { get; set; } = null!;

    public void Create(string workflowId, string createdBy, DateTime createdAt)
    {
        var workflowCreated = new WorkflowCreated(workflowId, createdBy, createdAt);
        Apply(workflowCreated);
        Add(workflowCreated);
    }

    private void Apply(WorkflowCreated workflowCreated)
    {
        if (WorkflowId == workflowCreated.WorkflowId)
        {
            throw new InvalidOperationException("Workflow with id already exists");
        }
        
        WorkflowId = workflowCreated.WorkflowId;
    }

    public override void When(IEvent ev)
    {
        switch (ev)
        {
            case WorkflowCreated workflowCreated:
                Apply(workflowCreated);
                break;
        }
    }
}