namespace AsmPj.Helpers;
using Serilog;
public class Result
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string? ErrorCode { get; private set; }

    protected Result(bool isSuccess, string? errorMessage = null, string? errorCode = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public static Result Success() => new(true);

    public static Result Failure(string message, string? code = null)
    {
        LogError(message, code);
        return new(false, message, code);
    }

    protected static void LogError(string message, string? code)
    {
        Log.Error("X Result Failure: {ErrorCode} - {Message}", code ?? "N/A", message);
    }
}

public class Result<T> : Result
{
    public T? Value { get; private set; }

    private Result(bool isSuccess, T? value = default, string? errorMessage = null, string? errorCode = null)
        : base(isSuccess, errorMessage, errorCode)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value);

    public new static Result<T> Failure(string message, string? code = null)
    {
        LogError(message, code);
        return new Result<T>(false, default, message, code);
    }
}