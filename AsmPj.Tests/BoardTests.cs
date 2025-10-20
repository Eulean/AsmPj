using AsmPj.Data;
using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Tests;

public class BoardTests
{
    private readonly IMapper _mapper;

    public BoardTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }
    private MyDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        
        return new MyDbContext(options);
    }

    [Fact]
    public async Task CreateBoard_ShouldAddBoardToDatabase()
    {
        // Arrange
        var context = GetDbContext();
        var createBoardRequest = new CreateBoardRequest
        {
            Name = "Test Board",
            Description = "This is a test board"
        };

        var board = _mapper.Map<Board>(createBoardRequest);
        // Manual Mapping
        // var board = new Board
        // {
        //     Name = createBoardRequest.Name,
        //     Description = createBoardRequest.Description,
        //     OwnerId = 1, // Assuming a user with ID 1 exists
        //     Created = DateTime.UtcNow
        // };
        
        
        // Act
        context.Boards.Add(board);
        await context.SaveChangesAsync();
        
        // Assert
        var saved = await context.Boards.FirstOrDefaultAsync(b => b.Name == "Test Board");
        Assert.NotNull(saved);
        Assert.Equal("Test Board", saved.Name);
    }
}