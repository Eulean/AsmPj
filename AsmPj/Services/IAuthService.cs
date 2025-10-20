using AsmPj.Helpers;
using AsmPj.Models;
using AsmPj.Models.Dtos;

namespace AsmPj.Services;

public interface IAuthService
{
    Task<Result<string>> RegisterAsync(RegisterRequest request);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
}