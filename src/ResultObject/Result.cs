using System.Diagnostics.CodeAnalysis;

namespace ResultObject;

/// <summary>
/// Represents a result of an operation that can either succeed with a value or fail with a categorized error.
/// </summary>
/// <typeparam name="TValue">The type of the value in case of success. Must be a non-nullable type.</typeparam>
/// <typeparam name="TErrorCategory">The enum type used for categorizing errors.</typeparam>
/// <remarks>
/// <para>
/// This class provides a concrete implementation of <see cref="IResult{TValue, TErrorCategory}"/>,
/// ensuring that an operation result contains either a success value or an error, but never both
/// or neither.
/// </para>
/// <para>
/// The class is designed to work with non-nullable reference types and includes compiler hints
/// through <see cref="MemberNotNullWhenAttribute"/> to support proper null checking.
/// </para>
/// </remarks>
/// <example>
/// Creating a success result:
/// <code>
/// var successResult = new Result&lt;Order, OrderErrorCategory&gt;(order, null);
/// </code>
/// 
/// Creating a failure result:
/// <code>
/// var error = new ResultError&lt;OrderErrorCategory&gt;("ORD001", "Invalid Order", "Order total cannot be negative");
/// var failureResult = new Result&lt;Order, OrderErrorCategory&gt;(null, error);
/// </code>
/// </example>
public class Result<TValue, TErrorCategory>(TValue? value, ResultError<TErrorCategory>? error)
    : IResult<TValue, TErrorCategory> where TValue : notnull where TErrorCategory : struct, Enum
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <remarks>
    /// This property will be true only when:
    /// <list type="bullet">
    /// <item><description>The error is null</description></item>
    /// <item><description>The value is not null</description></item>
    /// </list>
    /// When true, <see cref="Value"/> is guaranteed to be non-null and <see cref="Error"/> will be null.
    /// </remarks>
    /// <value>
    /// <c>true</c> if the operation succeeded and a value is present; otherwise, <c>false</c>.
    /// </value>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; } = error == null && value != null;

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    /// <remarks>
    /// This property will be true when either:
    /// <list type="bullet">
    /// <item><description>The error is not null</description></item>
    /// <item><description>The value is null</description></item>
    /// </list>
    /// When true, <see cref="Error"/> is guaranteed to be non-null and <see cref="Value"/> will be null.
    /// </remarks>
    /// <value>
    /// <c>true</c> if the operation failed and an error is present; otherwise, <c>false</c>.
    /// </value>
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsFailure { get; } = error != null || value == null;

    /// <summary>
    /// Gets the value produced by the operation if it was successful.
    /// </summary>
    /// <value>
    /// The operation result value if successful; otherwise, null.
    /// </value>
    public TValue? Value => value;

    /// <summary>
    /// Gets the error information if the operation failed.
    /// </summary>
    /// <value>
    /// The error information if the operation failed; otherwise, null.
    /// </value>
    public ResultError<TErrorCategory>? Error => error;

    /// <summary>
    /// Casts the success value to a different type if the operation was successful.
    /// </summary>
    /// <typeparam name="T">The type to cast the value to. Must be a non-nullable type.</typeparam>
    /// <returns>
    /// A new <see cref="Result{T, TErrorCategory}"/> containing either the cast value
    /// if successful, or the original error if the operation had failed.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// Thrown when the original result was successful but the value cannot be cast to type <typeparamref name="T"/>.
    /// </exception>
    /// <remarks>
    /// The method handles three scenarios:
    /// <list type="bullet">
    /// <item><description>If the result is a failure, returns a new failure result with the same error</description></item>
    /// <item><description>If the result is a success and the value can be cast, returns a new success result with the cast value</description></item>
    /// <item><description>If the result is a success but the value cannot be cast, throws an InvalidCastException</description></item>
    /// </list>
    /// </remarks>
    public Result<T, TErrorCategory> Cast<T>() where T : notnull
    {
        if (IsFailure)
        {
            // If it's a failure result, return a failure with the existing error.
            return new Result<T, TErrorCategory>(default, Error);
        }

        if (value is T castValue)
        {
            // Successfully cast the value to the target type T.
            return new Result<T, TErrorCategory>(castValue, null);
        }

        // The value could not be cast to the target type.
        throw new InvalidCastException(
            $"Cannot cast value of type {typeof(TValue).FullName} to {typeof(T).FullName}.");
    }
}

/// <summary>
/// Represents a result of an operation that can either succeed with a value or fail with a standard error category.
/// </summary>
/// <typeparam name="TValue">The type of the value in case of success. Must be a non-nullable type.</typeparam>
/// <remarks>
/// <para>
/// This class is a specialization of <see cref="Result{TValue, TErrorCategory}"/> that uses the standard
/// <see cref="ErrorCategory"/> enumeration for error categorization. It provides a simpler implementation
/// for scenarios where custom error categorization is not required.
/// </para>
/// <para>
/// All properties and methods behave identically to the generic class, but errors are always
/// categorized using the standard <see cref="ErrorCategory"/> enum.
/// </para>
/// </remarks>
/// <example>
/// Creating a success result:
/// <code>
/// var successResult = new Result&lt;Order&gt;(order, null);
/// </code>
/// 
/// Creating a failure result:
/// <code>
/// var error = new ResultError("ORD001", "Invalid Order", "Order total cannot be negative");
/// var failureResult = new Result&lt;Order&gt;(null, error);
/// </code>
/// </example>
public class Result<TValue>(TValue? value, ResultError<ErrorCategory>? error)
    : Result<TValue, ErrorCategory>(value, error) where TValue : notnull;