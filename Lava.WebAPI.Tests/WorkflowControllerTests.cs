using System;
using System.Threading.Tasks;
using FluentAssertions;
using Lava.Domain.Aggregates;
using Lava.Domain.Events;
using Lava.Domain.Interfaces;
using Lava.Domain.Tests;
using Lava.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Lava.WebAPI.Tests;

public class WorkflowControllerTests
{
    private WorkflowController _testSubject = null!;
    private IEventRepository _mockEventRepository = null!;

    private readonly string _workflowId = "workflow-id";
    private readonly string _createdBy = "created-by";
    private readonly DateTime _createdAt = new(2000, 1, 1);
    private Workflow _workflow = null!;
    
    [SetUp]
    public void Setup()
    {
        _mockEventRepository = Substitute.For<IEventRepository>();

        _workflow = new Workflow();
        _mockEventRepository
            .Get<Workflow>(Arg.Any<string>())
            .Returns(_workflow);
       
        _testSubject = new WorkflowController(_mockEventRepository);
    }

    [Test]
    public async Task Create_Returns_No_Content()
    {
        // arrange
        
        // act
        var actual = await _testSubject.Create(_workflowId, _createdBy, _createdAt);

        // assert
        actual.Should().BeOfType<NoContentResult>();
    }
    
    [Test]
    public async Task Create_Gets_Aggregate()
    {
        // arrange
        
        // act
        await _testSubject.Create(_workflowId, _createdBy, _createdAt);

        // assert
        await _mockEventRepository.Received(1).Get<Workflow>(_workflowId);
    }
    
    [Test]
    public async Task Create_Saves_Aggregate()
    {
        // arrange
        
        // act
        await _testSubject.Create(_workflowId, _createdBy, _createdAt);

        // assert
        await _mockEventRepository.Received(1).SaveChanges(Arg.Is<Workflow>(arg => IsValidWorkflow(arg, _workflow)));
    }

    private static bool IsValidWorkflow(Workflow actual, Workflow expected)
    {
        return actual.Should().Satisfy(should => should.BeEquivalentTo(expected));
    }
}