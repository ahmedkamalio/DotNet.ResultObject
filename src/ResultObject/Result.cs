using System.Diagnostics.CodeAnalysis;

namespace ResultObject;

/// <summary>
/// Represents the result of an operation, which can either be a success or a failure.
/// </summary>
/// <typeparam name="TValue">The type of the value contained in the result when it is successful.</typeparam>
public class Result<TValue>(TValue? value, ResultError? error) : IResult<TValue>
{
    /// <summary>
    /// Gets a value indicating whether the result represents a success.
    /// </summary>
    /// <remarks>
    /// This property is <c>true</c> if the <see cref="Value"/> is not <c>null</c> and there is no error.
    /// </remarks>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; } = error == null && value != null;

    /// <summary>
    /// Gets a value indicating whether the result represents a failure.
    /// </summary>
    /// <remarks>
    /// This property is <c>true</c> if the <see cref="Error"/> is not <c>null</c>.
    /// </remarks>
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsFailure { get; } = error != null || value == null;

    /// <summary>
    /// Gets the value of the result if it is a success; otherwise, <c>null</c>.
    /// </summary>
    public TValue? Value => value;

    /// <summary>
    /// Gets the error associated with the result if it is a failure; otherwise, <c>null</c>.
    /// </summary>
    public ResultError? Error => error;

    /// <summary>
    /// Casts the value of this result to a new type if the result is successful.
    /// </summary>
    /// <typeparam name="T">The new type to cast the value to.</typeparam>
    /// <returns>
    /// A new <see cref="Result{T}"/> with the value cast to the specified type, or the same error if the result was a failure.
    /// </returns>
    /// <exception cref="InvalidCastException">Thrown if the value cannot be cast to the specified type.</exception>
    public IResult<T> Cast<T>()
    {
        if (IsFailure)
        {
            // If it's a failure result, return a failure with the existing error.
            return new Result<T>(default, Error);
        }

        if (value is T castValue)
        {
            // Successfully cast the value to the target type T.
            return new Result<T>(castValue, null);
        }

        // The value could not be cast to the target type.
        throw new InvalidCastException(
            $"Cannot cast value of type {typeof(TValue).FullName} to {typeof(T).FullName}.");
    }
}