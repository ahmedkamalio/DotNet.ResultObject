using System.Diagnostics.CodeAnalysis;

namespace ResultObject;

/// <summary>
/// Represents a result of an operation that can either succeed with a value or fail with a categorized error.
/// </summary>
/// <typeparam name="TValue">The type of the value in case of success. Must be a non-nullable type.</typeparam>
/// <typeparam name="TErrorCategory">The enum type used for categorizing errors.</typeparam>
/// <remarks>
/// <para>
/// This interface provides a type-safe way to handle operation results, ensuring that either a value
/// or an error is present, but never both or neither. It supports custom error categorization through
/// the generic <typeparamref name="TErrorCategory"/> parameter.
/// </para>
/// <para>
/// The interface is designed to work with non-nullable reference types and includes appropriate
/// compiler hints through <see cref="MemberNotNullWhenAttribute"/> to support proper null checking.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public enum OrderErrorCategory
/// {
///     Validation,
///     Processing
/// }
/// 
/// public class OrderService
/// {
///     public IResult&lt;Order, OrderErrorCategory&gt; ProcessOrder(OrderRequest request)
///     {
///         if (!request.IsValid)
///             return Result.Failure&lt;Order, OrderErrorCategory&gt;(
///                 new ResultError&lt;OrderErrorCategory&gt;("INVALID_ORDER", "Validation Failed", "Invalid order details"));
///                 
///         // Process order...
///         return Result.Success&lt;Order, OrderErrorCategory&gt;(new Order());
///     }
/// }
/// </code>
/// </example>
public interface IResult<out TValue, TErrorCategory>
    where TValue : notnull
    where TErrorCategory : struct, Enum
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <remarks>
    /// When this property is true, <see cref="Value"/> will contain a non-null value
    /// and <see cref="Error"/> will be null. This is enforced through compiler attributes.
    /// </remarks>
    /// <value>
    /// <c>true</c> if the operation succeeded and a value is present; otherwise, <c>false</c>.
    /// </value>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    /// <remarks>
    /// When this property is true, <see cref="Error"/> will contain error information
    /// and <see cref="Value"/> will be null. This is enforced through compiler attributes.
    /// </remarks>
    /// <value>
    /// <c>true</c> if the operation failed and an error is present; otherwise, <c>false</c>.
    /// </value>
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    bool IsFailure { get; }

    /// <summary>
    /// Gets the value produced by the operation if it was successful.
    /// </summary>
    /// <remarks>
    /// This property will be non-null only when <see cref="IsSuccess"/> is true.
    /// The compiler enforces this through the <see cref="MemberNotNullWhenAttribute"/>.
    /// </remarks>
    /// <value>
    /// The operation result value if successful; otherwise, null.
    /// </value>
    TValue? Value { get; }

    /// <summary>
    /// Gets the error information if the operation failed.
    /// </summary>
    /// <remarks>
    /// This property will be non-null only when <see cref="IsFailure"/> is true.
    /// The compiler enforces this through the <see cref="MemberNotNullWhenAttribute"/>.
    /// </remarks>
    /// <value>
    /// The error information if the operation failed; otherwise, null.
    /// </value>
    ResultError<TErrorCategory>? Error { get; }

    /// <summary>
    /// Casts the success value to a different type if the operation was successful.
    /// </summary>
    /// <typeparam name="T">The type to cast the value to. Must be a non-nullable type.</typeparam>
    /// <returns>
    /// A new <see cref="Result{T, TErrorCategory}"/> containing either the cast value
    /// if successful, or the original error if the operation had failed.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// Thrown when the cast is invalid and the original result was successful.
    /// </exception>
    /// <remarks>
    /// This method preserves the result state:
    /// <list type="bullet">
    /// <item><description>If the original result is a failure, returns a failure with the same error.</description></item>
    /// <item><description>If the original result is a success, attempts to cast the value to type <typeparamref name="T"/>.</description></item>
    /// </list>
    /// </remarks>
    Result<T, TErrorCategory> Cast<T>() where T : notnull;
}

/// <summary>
/// Represents a result of an operation that can either succeed with a value or fail with a standard error category.
/// </summary>
/// <typeparam name="TValue">The type of the value in case of success. Must be a non-nullable type.</typeparam>
/// <remarks>
/// <para>
/// This interface is a specialization of <see cref="IResult{TValue, TErrorCategory}"/> that uses the standard
/// <see cref="ErrorCategory"/> enumeration for error categorization. It provides a simpler interface for
/// scenarios where custom error categorization is not required.
/// </para>
/// <para>
/// All properties and methods behave identically to the generic interface, but errors are always
/// categorized using the standard <see cref="ErrorCategory"/> enum.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UserService
/// {
///     public IResult&lt;User&gt; GetUser(string id)
///     {
///         var user = _repository.FindUser(id);
///         if (user == null)
///             return Result.Failure&lt;User&gt;(
///                 new ResultError("USER_NOT_FOUND", "Not Found", "User does not exist"));
///                 
///         return Result.Success(user);
///     }
/// }
/// </code>
/// </example>
public interface IResult<out TValue> : IResult<TValue, ErrorCategory>
    where TValue : notnull;