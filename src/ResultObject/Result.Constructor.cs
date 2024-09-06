namespace ResultObject;

/// <summary>
/// Helper class to create Result instances with type inference.
/// </summary>
public static class Result
{
    public static Result<object> Success()
    {
        return new Result<object>(default, null);
    }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value, null);
    }

    public static Result<T> Failure<T>(ResultError error)
    {
        return new Result<T>(default, error);
    }

    public static Result<T> Failure<T>(string code, string reason, string message)
    {
        return new Result<T>(default, new ResultError(code, reason, message));
    }
}
