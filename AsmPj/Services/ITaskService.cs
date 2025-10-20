using AsmPj.Helpers;
using AsmPj.Models.Dtos;

namespace AsmPj.Services;

public interface ITaskService
{
    Task<Result<TaskResponse>> CreateTaskAsync(TaskRequest request);
    Task<Result<IEnumerable<TaskResponse>>> GetAllTasksAsync();
    Task<Result<TaskResponse>> GetTaskByIdAsync(int taskId);
    Task<Result<IEnumerable<TaskResponse>>> GetTasksByBoardIdAsync(int boardId);
    Task<Result<TaskResponse>> UpdateTaskAsync(int taskId, TaskRequest request);
    Task<Result<string>> DeleteTaskAsync(int taskId);
}