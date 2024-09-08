namespace ResultObject;

/// <summary>
/// Represents the result of an operation, which can either be a success or a failure.
/// </summary>
/// <typeparam name="T">The type of the value in case of a successful result.</typeparam>
public partial class Result<T>(T? value, ResultError? error)
{
    /// <summary>
    /// Gets a value indicating whether the result is successful.
    /// </summary>
    public bool IsSuccess => Error is null;

    /// <summary>
    /// Gets a value indicating whether the result is a failure.
    /// </summary>
    public bool IsFailure => Error is not null;

    /// <summary>
    /// Gets the value associated with a successful result, or <c>null</c> if the result is a failure.
    /// </summary>
    public T? Value { get; } = value;

    /// <summary>
    /// Gets the error details associated with a failed result, or <c>null</c> if the result is successful.
    /// </summary>
    public ResultError? Error { get; } = error;

    /// <summary>
    /// <para>
    /// Creates a new <c>Result</c> instance representing the same failure as this result, 
    /// but with the value type cast to a different type.
    /// </para>
    /// <para>
    /// The new result will contain the same error information,
    /// but the value will be set to the default value of the new type.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The new type of the value in the returned <c>Result</c>.</typeparam>
    /// <returns>A new <c>Result</c> instance representing the failure, with the value type changed.</returns>
    public Result<TValue> ToFailureResult<TValue>() => new(default, Error);

    /// <summary>
    /// Retrieves the value from the <see cref="Result{T}"/>, throwing an exception if the result is unsuccessful or the value is null.
    /// </summary>
    /// <remarks>
    /// This method should be used in scenarios where the result's value is critical to the operation, and you expect it to be non-null. 
    /// If the result is a failure or the value is null, an <see cref="InvalidOperationException"/> is thrown, ensuring that invalid states are not silently handled.
    /// <br />
    /// This is recommended for business-critical logic where you need strict guarantees that the value is present and valid.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the result is a failure or if the value is null in a successful result.
    /// </exception>
    /// <returns>The value of the result if successful and non-null.</returns>
    public T MustGetValue()
    {
        if (IsFailure)
        {
            throw new InvalidOperationException("Cannot retrieve value from a failed result.");
        }

        if (Value is null)
        {
            throw new InvalidOperationException("Result was successful but the value is null.");
        }

        return Value;
    }
}