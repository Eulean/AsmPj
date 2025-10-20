using System.Security.Claims;
using AsmPj.Helpers;
using AsmPj.Models.Dtos;
using AsmPj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsmPj.Controllers;

/// <summary>
///     Handle Board Management (Create, Get My Boards, Update, Delete)
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BoardController : Controller
{
    private readonly IBoardService _boardService;

    public BoardController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    /// <summary>
    ///     Create a new board.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var result = await _boardService.CreateBoardAsync(request, int.Parse(userIdClaim));

        return result.ToActionResult();
    }

    /// <summary>
    ///     Get all boards of the authenticated user.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetMyBoards()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var result = await _boardService.GetAllBoardsByUserIdAsync(int.Parse(userIdClaim));
        return result.ToActionResult();
    }

    /// <summary>
    ///     Update an existing board.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBoard(int id, [FromBody] UpdateBoardRequest request)
    {
        var result = await _boardService.UpdateBoardAsync(id, request);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Delete a board.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoard(int id)
    {
        var result = await _boardService.DeleteBoardAsync(id);
        return result.ToActionResult();
    }
}