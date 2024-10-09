namespace ResultObject;

/// <summary>
/// Helper class to create <see cref="Result{TValue}"/> instances with type inference.
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the result value.</typeparam>
    /// <param name="value">The value associated with a successful result.</param>
    /// <returns>A successful <see cref="Result{TValue}"/> instance containing the provided value.</returns>
    public static Result<TValue> Success<TValue>(TValue value) => new(value, null);

    /// <summary>
    /// Creates a failed result with the specified error information.
    /// </summary>
    /// <typeparam name="TValue">The type of the result value, which will be <c>null</c> in case of failure.</typeparam>
    /// <param name="error">A <see cref="ResultError"/> containing detailed information about the failure.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> instance with no value and the provided error.</returns>
    public static Result<TValue> Failure<TValue>(ResultError error) => new(default, error);

    /// <summary>
    /// Creates a failed result with the specified error code, reason, and message.
    /// </summary>
    /// <typeparam name="TValue">The type of the result value, which will be <c>null</c> in case of failure.</typeparam>
    /// <param name="code">A code representing the type of failure.</param>
    /// <param name="reason">A brief description of the failure reason.</param>
    /// <param name="message">A detailed message explaining the failure.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> instance with no value and an error containing the specified details.</returns>
    public static Result<TValue> Failure<TValue>(string code, string reason, string message) =>
        new(default, new ResultError(code, reason, message));
}