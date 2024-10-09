namespace ResultObject;

/// <summary>
/// Represents the result of an operation, which can either be a success with a value or a failure with an error.
/// </summary>
/// <typeparam name="TValue">The type of the value associated with a successful result.</typeparam>
public class Result<TValue>(TValue? value, ResultError? error)
{
    /// <summary>
    /// Gets a value indicating whether the result represents a successful outcome (i.e., no error occurred).
    /// </summary>
    public bool IsSuccess { get; } = error == null;

    /// <summary>
    /// Gets a value indicating whether the result represents a failure (i.e., an error occurred).
    /// </summary>
    public bool IsFailure { get; } = error != null;

    /// <summary>
    /// Gets the value of a successful result.
    /// Throws an exception if the result represents a failure or if the value is null even though the result is marked as successful.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when trying to access the value of a failed result, or when the result is successful but the value is null.
    /// </exception>
    public TValue Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException("Cannot retrieve value from a failed result.");
            }

            if (value == null)
            {
                throw new InvalidOperationException("Value is null despite the operation being successful.");
            }

            return value;
        }
    }

    /// <summary>
    /// Gets the value if the result is successful, or the default value for the type <typeparamref name="TValue"/> if the result is a failure.
    /// </summary>
    public TValue? ValueOrDefault => value;

    /// <summary>
    /// Gets the error associated with a failed result.
    /// Throws an exception if accessed on a successful result.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when trying to access the error of a successful result.
    /// </exception>
    public ResultError Error
    {
        get
        {
            if (IsSuccess)
            {
                throw new InvalidOperationException("Cannot retrieve error from a successful result.");
            }

            return error!;
        }
    }

    /// <summary>
    /// Gets the error if the result is a failure, or null if the result is successful.
    /// </summary>
    public ResultError? ErrorOrDefault => error;

    /// <summary>
    /// Creates a new <c>Result</c> instance representing the same failure as this result,
    /// but with the value type cast to a different type.
    /// The new result will contain the same error, while the value will be set to the default value of the new type.
    /// </summary>
    /// <typeparam name="T">The new type for the value in the returned <c>Result</c>.</typeparam>
    /// <returns>A new <c>Result</c> instance representing the failure, with the value type changed to <typeparamref name="T"/>.</returns>
    public Result<T> ToFailureResult<T>() => new(default, Error);
}