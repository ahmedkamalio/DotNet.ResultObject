namespace ResultObject;

/// <summary>
/// Helper class to create <see cref="Result{T}"/> instances with type inference.
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a successful result with a default value of <see cref="object"/>.
    /// </summary>
    /// <returns>A successful <see cref="Result{T}"/> instance with a value of <c>null</c>.</returns>
    public static Result<object> Success() => new(default, null);

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="value">The value of the successful result.</param>
    /// <returns>A successful <see cref="Result{T}"/> instance containing the provided value.</returns>
    public static Result<T> Success<T>(T value) => new(value, null);

    /// <summary>
    /// Creates a failed result with the specified error information.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="error">A <see cref="ResultError"/> that contains detailed information about the failure.</param>
    /// <returns>A failed <see cref="Result{T}"/> instance with no value and the provided error.</returns>
    public static Result<T> Failure<T>(ResultError error) => new(default, error);

    /// <summary>
    /// Creates a failed result with the specified error code, reason, and message.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="code">The error code representing the type of failure.</param>
    /// <param name="reason">A short description of the failure reason.</param>
    /// <param name="message">A detailed message explaining the error.</param>
    /// <returns>A failed <see cref="Result{T}"/> instance with no value and the provided error information.</returns>
    public static Result<T> Failure<T>(string code, string reason, string message) =>
        new(default, new ResultError(code, reason, message));
}