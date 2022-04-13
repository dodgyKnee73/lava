using Lava.Domain.Aggregates;
using Lava.Domain.Interfaces;
using Lava.WebAPI.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Lava.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkflowController : ControllerBase
{
    private readonly IEventRepository _eventRepository;

    public WorkflowController(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    [HttpPost]
    [Route("/create")]
    public async Task<IActionResult> Create(string workflowId, string createdBy, DateTime createdAt)
    {
        var command = new CreateWorkflow(workflowId, createdBy, createdAt);
        var workflow = await _eventRepository.Get<Workflow>(command.WorkflowId);
        workflow.Create(command.WorkflowId, command.CreatedBy, command.CreatedAt);
        await _eventRepository.SaveChanges(workflow);
        return NoContent();
    }
}
