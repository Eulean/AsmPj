using AsmPj.Data;
using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsmPj.Services;

public class TaskService : ITaskService
{
    private readonly MyDbContext _context;
    private readonly ILogger<TaskService> _logger;
    private readonly IMapper _mapper;
    private readonly IActivityLogService _activityLogService;
    private readonly IUserContext _userContext;
    
    public TaskService(MyDbContext context, ILogger<TaskService> logger, IMapper mapper,IActivityLogService activityLogService,IUserContext userContext)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _activityLogService = activityLogService;
        _userContext = userContext;
    }


    public async Task<Result<TaskResponse>> CreateTaskAsync(TaskRequest request)
    {
        try
        {
            var task = _mapper.Map<TaskItem>(request);
            task.Created = DateTime.UtcNow;
            
            
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            
            var response = _mapper.Map<TaskResponse>(task);
            _logger.LogInformation("Task created successfully with ID {TaskId}", task.Id);

            var userEmail = _userContext.GetUserEmail() ?? "System";
            
            await _activityLogService.LogAsync(
                action:$"Created Task with ID {task.Id}",
                performedBy: userEmail,
                entityType: "TaskItem",
                entityId: task.Id
                
            );
            return Result<TaskResponse>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating task");
            return Result<TaskResponse>.Failure("Error creating task", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<IEnumerable<TaskResponse>>> GetAllTasksAsync()
    {
        try
        {
            var tasks = await _context.TaskItems
                .AsNoTracking()
                .Include(t => t.Board)
                .ToListAsync();
            
            var response = _mapper.Map<IEnumerable<TaskResponse>>(tasks);
            _logger.LogInformation("Fetched all tasks successfully");
            
            return Result<IEnumerable<TaskResponse>>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching all tasks");
            return Result<IEnumerable<TaskResponse>>.Failure("Error fetching tasks", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<TaskResponse>> GetTaskByIdAsync(int taskId)
    {
        try
        {
            var task = await _context.TaskItems
                .AsNoTracking()
                .Include(t => t.Board)
                .Where(t => t.Id == taskId)
                .FirstOrDefaultAsync();
            
            var response = _mapper.Map<TaskResponse>(task);
            
            _logger.LogInformation("Fetched task with ID {TaskId} successfully", taskId);
            return Result<TaskResponse>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching task with ID {TaskId}", taskId);
            return Result<TaskResponse>.Failure("Error fetching task", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<IEnumerable<TaskResponse>>> GetTasksByBoardIdAsync(int boardId)
    {
        try
        {
            var tasks = await _context.TaskItems
                .AsNoTracking()
                .Where(t => t.BoardId == boardId)
                .ToListAsync();
            
            var response = _mapper.Map<IEnumerable<TaskResponse>>(tasks);
            _logger.LogInformation("Fetched tasks for board ID {BoardId} successfully", boardId);
            return Result<IEnumerable<TaskResponse>>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching tasks for board ID {BoardId}", boardId);
            return Result<IEnumerable<TaskResponse>>.Failure("Error fetching tasks", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<TaskResponse>> UpdateTaskAsync(int taskId, TaskRequest request)
    {
        try
        {
            var task = await _context.TaskItems.FindAsync(taskId);
            if (task == null)
                return Result<TaskResponse>.Failure("Task not found", AppErrorCodes.TaskNotFound);
            
            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = request.Status;
            task.Modified = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            var response = _mapper.Map<TaskResponse>(task);
            _logger.LogInformation("Task with ID {TaskId} updated successfully", taskId);
            
            var userEmail = _userContext.GetUserEmail() ?? "System";
            
            await _activityLogService.LogAsync(
                action:$"Updated Task with ID {task.Id}",
                performedBy: userEmail,
                entityType: "TaskItem",
                entityId: task.Id
                
            );
            
            return Result<TaskResponse>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating task with ID {TaskId}", taskId);
            return Result<TaskResponse>.Failure("Error updating task", AppErrorCodes.ServerError);
        }
    }

    public async Task<Result<string>> DeleteTaskAsync(int taskId)
    {
        try
        {
            var task = await _context.TaskItems.FindAsync(taskId);
            if (task == null)
                return Result<string>.Failure("Task not found", AppErrorCodes.TaskNotFound);
            
            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Task with ID {TaskId} deleted successfully", taskId);
           
            var userEmail = _userContext.GetUserEmail() ?? "System";
            
            await _activityLogService.LogAsync(
                action:$"Deleted Task with ID {task.Id}",
                performedBy: userEmail,
                entityType: "TaskItem",
                entityId: task.Id
                
            );
            
            return Result<string>.Success("Task deleted successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting task with ID {TaskId}", taskId);
            return Result<string>.Failure("Error deleting task", AppErrorCodes.ServerError);
        }
    }
}