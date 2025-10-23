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

public class BoardServiceTests
{
    private readonly IMapper _mapper;

    public BoardServiceTests()
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
    public async Task CreateBoard_ShouldAddBoard()
    {
        // Arrange 
        var context = GetDbContext();
        var loggerMock = new Mock<ILogger<BoardService>>();
        var activityLogServiceMock = new Mock<IActivityLogService>();
        var userContextMock = new Mock<IUserContext>();
        
        userContextMock.Setup(u => u.GetUserEmail()).Returns("testuser@example.com");

        
        var boardService = new BoardService(
            context,
            loggerMock.Object,
            _mapper,
            activityLogServiceMock.Object,
            userContextMock.Object
        );

        var request = new CreateBoardRequest
        {
            Name = "Test Board",
            Description = "Test Board Description",
        };

        var userId = 1;
        
        // Act 
        var result = await boardService.CreateBoardAsync(request, userId);
        
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Test Board");
        result.Value.Description.Should().Be("Test Board Description");

        var savedBoard = await context.Boards.FirstOrDefaultAsync(b => b.OwnerId == userId);
        
        savedBoard.Should().NotBeNull();
        savedBoard.Name.Should().Be("Test Board");
        savedBoard.Description.Should().Be("Test Board Description");
        
        // Verify logging and activity log
        activityLogServiceMock.Verify(a => 
            a.LogAsync(
                It.Is<string>(s => s.Contains("Created Board")),
                "testuser@example.com",
                "Board",
                It.IsAny<int>()
                ),
            Times.Once);
    }
    
    [Fact]
    public async Task UpdateBoardAsync_Should_Update_Board_And_Log_Activity()
    {
        // Arrange
        var context = GetDbContext();
        var loggerMock = new Mock<ILogger<BoardService>>();
        var activityLogServiceMock = new Mock<IActivityLogService>();
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetUserEmail()).Returns("testuser@example.com");

        var board = new Board
        {
            Id = 1,
            Name = "Old Board",
            Description = "Old Desc",
            OwnerId = 1,
            Created = DateTime.UtcNow
        };
        context.Boards.Add(board);
        await context.SaveChangesAsync();

        var service = new BoardService(
            context,
            loggerMock.Object,
            _mapper,
            activityLogServiceMock.Object,
            userContextMock.Object
        );

        var updateRequest = new UpdateBoardRequest
        {
            Name = "Updated Board",
            Description = "Updated Desc"
        };

        // Act
        var result = await service.UpdateBoardAsync(board.Id, updateRequest);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Updated Board");

        var updated = await context.Boards.FindAsync(board.Id);
        updated.Should().NotBeNull();
        updated!.Description.Should().Be("Updated Desc");

        activityLogServiceMock.Verify(a =>
                a.LogAsync(
                    It.Is<string>(s => s.Contains("Updated Board")),
                    "testuser@example.com",
                    "Board",
                    board.Id
                ),
            Times.Once);
    }

}