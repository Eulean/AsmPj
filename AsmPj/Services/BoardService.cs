using AsmPj.Data;
using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Services;

public class BoardService : IBoardService
{
    private readonly MyDbContext _context;
    private readonly ILogger<BoardService> _logger;
    private readonly IMapper _mapper;
    private readonly IActivityLogService _activityLogService;
    private readonly IUserContext _userContext;

    public BoardService(MyDbContext context, ILogger<BoardService> logger, IMapper mapper, IActivityLogService activityLogService, IUserContext userContext)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _activityLogService = activityLogService;
        _userContext = userContext;
    }

    public async Task<Result<BoardResponse>> CreateBoardAsync(CreateBoardRequest request, int userId)
    {
        try
        {
            var board = _mapper.Map<Board>(request);
            board.OwnerId = userId;
            board.Created = DateTime.UtcNow;

            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<BoardResponse>(board);
            
            _logger.LogInformation("Board created successfully for user {UserId}", userId);
            
            var userEmail = _userContext.GetUserEmail() ?? "System";
            await _activityLogService.LogAsync(
                action: $"Created Board with ID {board.Id}",
                performedBy: userEmail,
                entityType: "Board",
                entityId: board.Id
            );
            
            return Result<BoardResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating board for user {UserId}", userId);
            return Result<BoardResponse>.Failure("Error Creating Board", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<IEnumerable<BoardResponse>>> GetBoardByIdAndUserIdAsync(int userId, int boardId)
    {
        try
        {
            var board = await _context.Boards
                .AsNoTracking()
                .Where(b => b.OwnerId == userId && b.Id == boardId)
                .Include(b => b.Tasks)
                .ToListAsync();

            var reesponse = _mapper.Map<IEnumerable<BoardResponse>>(board);
            _logger.LogInformation("Fetched board {BoardId} for user {UserId}", boardId, userId);
            return Result<IEnumerable<BoardResponse>>.Success(reesponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching board {BoardId} for user {UserId}", boardId, userId);
            return Result<IEnumerable<BoardResponse>>.Failure("Error fetching boards", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<IEnumerable<BoardResponse>>> GetAllBoardsByUserIdAsync(int userId)
    {
        try
        {
            var boards = await _context.Boards
                .AsNoTracking()
                .Where(b => b.OwnerId == userId)
                .Include(b => b.Tasks)
                .ToListAsync();

            var response = _mapper.Map<IEnumerable<BoardResponse>>(boards);
            
            _logger.LogInformation("Fetched all boards for user {UserId}", userId);
            return Result<IEnumerable<BoardResponse>>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching boards for user {UserId}", userId);
            return Result<IEnumerable<BoardResponse>>.Failure("Error fetching boards", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<IEnumerable<BoardResponse>>> GetBoardByIdAsync(int userId)
    {
        try
        {
            var boards = await _context.Boards
                .AsNoTracking()
                .Where(b => b.OwnerId == userId)
                .Include(b => b.Tasks)
                .ToListAsync();

            var response = _mapper.Map<IEnumerable<BoardResponse>>(boards);
            
            _logger.LogInformation("Fetched boards for user {UserId}", userId);
            return Result<IEnumerable<BoardResponse>>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching boards for user {UserId}", userId);
            return Result<IEnumerable<BoardResponse>>.Failure("Error fetching boards", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<IEnumerable<BoardResponse>>> GetAllBoardsAsync()
    {
        try
        {
            var boards = await _context.Boards
                .AsNoTracking()
                .Include(b => b.Tasks)
                .ToListAsync();

            var response = _mapper.Map<IEnumerable<BoardResponse>>(boards);

            _logger.LogInformation("Fetched all boards");
            return Result<IEnumerable<BoardResponse>>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching all boards");
            return Result<IEnumerable<BoardResponse>>.Failure("Error fetching boards", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<BoardResponse>> UpdateBoardAsync(int boardId, UpdateBoardRequest request)
    {
        try
        {
            var board = await _context.Boards.FindAsync(boardId);
            if (board == null)
                return Result<BoardResponse>.Failure("Board not found", AppErrorCodes.BoardNotFound);

            board.Name = request.Name;
            board.Description = request.Description;
            board.Modified = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            var response = _mapper.Map<BoardResponse>(board);
            _logger.LogInformation("Board {BoardId} updated successfully", boardId);
           
            var userEmail = _userContext.GetUserEmail() ?? "System";
            await _activityLogService.LogAsync(
                action: $"Updated Board with ID {board.Id}",
                performedBy: userEmail,
                entityType: "Board",
                entityId: board.Id
            );
            
            return Result<BoardResponse>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating board {BoardId}", boardId);
            return Result<BoardResponse>.Failure("Error updating board", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<string>> DeleteBoardAsync(int boardId)
    {
        try
        {
            var board =  await _context.Boards.FindAsync(boardId);
            if (board == null)
                return Result<string>.Failure("Board not found", AppErrorCodes.BoardNotFound);
            
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Board {BoardId} deleted successfully", boardId);
            
            var userEmail = _userContext.GetUserEmail() ?? "System";
            await _activityLogService.LogAsync(
                action: $"Deleted Board with ID {board.Id}",
                performedBy: userEmail,
                entityType: "Board",
                entityId: board.Id
            );
            
            return Result<string>.Success("Board deleted successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting board {BoardId}", boardId);
            return Result<string>.Failure("Error deleting board", AppErrorCodes.ServerError);
        }
    }
}