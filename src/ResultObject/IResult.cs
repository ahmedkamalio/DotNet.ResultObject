using System.Diagnostics.CodeAnalysis;

namespace ResultObject;

/// <summary>
/// Represents a result of an operation, providing information about success or failure.
/// </summary>
public interface IResult<out TValue>
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    bool IsFailure { get; }

    /// <summary>
    /// Gets the value returned by the operation if it was successful.
    /// </summary>
    TValue? Value { get; }

    /// <summary>
    /// Gets the error information associated with the failure, if any.
    /// </summary>
    ResultError? Error { get; }

    /// <summary>
    /// Casts the value of this result to a new type if the result is successful.
    /// </summary>
    /// <typeparam name="T">The new type to cast the value to.</typeparam>
    /// <returns>
    /// A new <see cref="Result{T}"/> with the value cast to the specified type, or the same error if the result was a failure.
    /// </returns>
    /// <exception cref="InvalidCastException">Thrown if the value cannot be cast to the specified type.</exception>
    IResult<T> Cast<T>();
}