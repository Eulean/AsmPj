using System.Security.Claims;
using AsmPj.Helpers;
using AsmPj.Models.Dtos;
using AsmPj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsmPj.Controllers;

/// <summary>
///     Handle Task Management (Create, Get Tasks by Board, Update, Delete)
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController : Controller
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    ///     Create a new task.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var result = await _taskService.CreateTaskAsync(request);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Get all tasks by board ID.
    /// </summary>
    /// <param name="boardId"></param>
    /// <returns></returns>
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetTasksByBoard(int boardId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var result = await _taskService.GetTasksByBoardIdAsync(boardId);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Update an existing task.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var result = await _taskService.UpdateTaskAsync(id, request);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Delete a task.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var result = await _taskService.DeleteTaskAsync(id);
        return result.ToActionResult();
    }
}