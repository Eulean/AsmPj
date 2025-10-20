using AsmPj.Helpers;
using AsmPj.Models.Dtos;

namespace AsmPj.Services;

public interface IBoardService
{
   Task<Result<BoardResponse>> CreateBoardAsync(CreateBoardRequest request, int userId);
   Task<Result<IEnumerable<BoardResponse>>> GetBoardByIdAndUserIdAsync(int userId, int boardId);
   Task<Result<IEnumerable<BoardResponse>>> GetAllBoardsByUserIdAsync(int userId);
   Task<Result<IEnumerable<BoardResponse>>> GetBoardByIdAsync(int userId);
   Task<Result<IEnumerable<BoardResponse>>> GetAllBoardsAsync();
   Task<Result<BoardResponse>> UpdateBoardAsync(int boardId, UpdateBoardRequest request);
   Task<Result<string>> DeleteBoardAsync(int boardId);

}