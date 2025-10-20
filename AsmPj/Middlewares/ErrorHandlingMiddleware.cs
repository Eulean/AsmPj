using AsmPj.Helpers;

namespace AsmPj.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            
            var result = Result.Failure("An unexpected error occurred.", AppErrorCodes.ServerError);
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}