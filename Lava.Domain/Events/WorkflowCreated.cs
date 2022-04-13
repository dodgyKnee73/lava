namespace Lava.Domain.Events;

public record WorkflowCreated(
    string WorkflowId,
    string CreatedBy,
    DateTime CreatedAt
    ) : AbstractEvent("workflow-created")
{
    public override string StreamId => WorkflowId;
}