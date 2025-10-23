using Microsoft.AspNetCore.Mvc;

namespace AsmPj.Helpers;

public static class ResultExtensions 
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new ObjectResult(new 
            {
                success = true,
                data = result.Value
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        var status = (result.ErrorCode ?? string.Empty).GetStatusCode();
        return new ObjectResult(new
        {
            success = false,
            error = new
            {
                message = result.ErrorMessage,
                code = result.ErrorCode
            }
        })
        {
            StatusCode = status
        };
    }


    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new ObjectResult(new
            {
                success = true
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        var status = (result.ErrorCode ?? string.Empty).GetStatusCode();
        return new ObjectResult(new
        {
            success = false,
            error = new
            {
                message = result.ErrorMessage,
                code = result.ErrorCode
            }
        })
        {
            StatusCode = status
        };
    }

    public static int GetStatusCode(this string code)
    {
        return code switch
        {
            AppErrorCodes.InvalidCredentials => StatusCodes.Status401Unauthorized,
            AppErrorCodes.ServerError => StatusCodes.Status500InternalServerError,
            AppErrorCodes.DatabaseError => StatusCodes.Status500InternalServerError,
            AppErrorCodes.UnauthorizedAccess => StatusCodes.Status403Forbidden,
            AppErrorCodes.UserNotFound => StatusCodes.Status404NotFound,
            AppErrorCodes.NotFound => StatusCodes.Status404NotFound,

            // specific Service codes
            AppErrorCodes.EmailExists => StatusCodes.Status400BadRequest,
            AppErrorCodes.RoleNotFound => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status400BadRequest
        };
    }
}