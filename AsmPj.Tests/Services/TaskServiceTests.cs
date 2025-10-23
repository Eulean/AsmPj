using AsmPj.Data;
using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;
using AsmPj.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace AsmPj.Tests;

public class TaskServiceTests
{
    private readonly IMapper _mapper;

    public TaskServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    private MyDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContext = new MyDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }
    
    [Fact]
    public async Task CreateTaskAsync_Should_Create_Task_And_Log_Activity()
    {
        // Arrange
        var context = GetDbContext();
        var loggerMock = new Mock<ILogger<TaskService>>();
        var activityLogMock = new Mock<IActivityLogService>();
        var userContextMock = new Mock<IUserContext>();

        userContextMock.Setup(u => u.GetUserEmail()).Returns("tester@example.com");

        var service = new TaskService(
            context,
            loggerMock.Object,
            _mapper,
            activityLogMock.Object,
            userContextMock.Object
        );

        var request = new TaskRequest
        {
            Title = "Unit Test Task",
            Description = "Testing creation",
            Status = "Pending",
            BoardId = 1
        };

        // Act
        var result = await service.CreateTaskAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Unit Test Task");

        var task = await context.TaskItems.FirstOrDefaultAsync();
        task.Should().NotBeNull();
        task!.Description.Should().Be("Testing creation");

        activityLogMock.Verify(a =>
                a.LogAsync(
                    It.Is<string>(s => s.Contains("Created Task")),
                    "tester@example.com",
                    "TaskItem",
                    task.Id
                ),
            Times.Once);
    }
    
    [Fact]
    public async Task GetAllTasksAsync_Should_Return_All_Tasks()
    {
        // Arrange
        var context = GetDbContext();
        context.Boards.Add(new Board { Id = 1, Name = "Board 1" });
        context.Boards.Add(new Board { Id = 2, Name = "Board 2" });
        
        context.TaskItems.AddRange(new List<TaskItem>
        {
            new() { Title = "Task A", Status = "Pending", BoardId = 1, Created = DateTime.UtcNow },
            new() { Title = "Task B", Status = "Completed", BoardId = 2, Created = DateTime.UtcNow }
        });
        await context.SaveChangesAsync();

        var service = new TaskService(
            context,
            Mock.Of<ILogger<TaskService>>(),
            _mapper,
            Mock.Of<IActivityLogService>(),
            Mock.Of<IUserContext>()
        );

        // Act
        var result = await service.GetAllTasksAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetTaskByIdAsync_Should_Return_Task()
    {
        // Arrange
        var context = GetDbContext();
        context.Boards.Add(new Board { Id = 1, Name = "Board 1" });
        var task = new TaskItem { Title = "Task X", Status = "Pending",BoardId = 1, Created = DateTime.UtcNow };
        context.TaskItems.Add(task);
        await context.SaveChangesAsync();

        var service = new TaskService(
            context,
            Mock.Of<ILogger<TaskService>>(),
            _mapper,
            Mock.Of<IActivityLogService>(),
            Mock.Of<IUserContext>()
        );

        // Act
        var result = await service.GetTaskByIdAsync(task.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("Task X");
    }
    
    [Fact]
    public async Task UpdateTaskAsync_Should_Update_Task_And_Log_Activity()
    {
        // Arrange
        var context = GetDbContext();
        var existing = new TaskItem { Id = 1, Title = "Old Task", Description = "Old Desc", Status = "Pending" };
        context.TaskItems.Add(existing);
        await context.SaveChangesAsync();

        var activityLogMock = new Mock<IActivityLogService>();
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetUserEmail()).Returns("user@example.com");

        var service = new TaskService(
            context,
            Mock.Of<ILogger<TaskService>>(),
            _mapper,
            activityLogMock.Object,
            userContextMock.Object
        );

        var update = new TaskRequest()
        {
            Title = "Updated Task",
            Description = "New Desc",
            Status = "Completed",
            BoardId = 1
        };

        // Act
        var result = await service.UpdateTaskAsync(1, update);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Updated Task");

        var updated = await context.TaskItems.FindAsync(1);
        updated!.Description.Should().Be("New Desc");

        activityLogMock.Verify(a =>
                a.LogAsync(It.Is<string>(s => s.Contains("Updated Task")),
                    "user@example.com", "TaskItem", 1),
            Times.Once);
    }
    
    [Fact]
    public async Task DeleteTaskAsync_Should_Remove_Task_And_Log_Activity()
    {
        // Arrange
        var context = GetDbContext();
        var task = new TaskItem() { Id = 1, Title = "Delete Me" };
        context.TaskItems.Add(task);
        await context.SaveChangesAsync();

        var activityLogMock = new Mock<IActivityLogService>();
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetUserEmail()).Returns("deleter@example.com");

        var service = new TaskService(
            context,
            Mock.Of<ILogger<TaskService>>(),
            _mapper,
            activityLogMock.Object,
            userContextMock.Object
        );

        // Act
        var result = await service.DeleteTaskAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await context.TaskItems.CountAsync()).Should().Be(0);

        activityLogMock.Verify(a =>
                a.LogAsync(It.Is<string>(s => s.Contains("Deleted Task")),
                    "deleter@example.com", "TaskItem", 1),
            Times.Once);
    }
}


