namespace ResultObject;

/// <summary>
/// Provides static factory methods for creating Result instances with type inference support.
/// </summary>
/// <remarks>
/// This class simplifies the creation of Result objects by providing convenient factory methods
/// that handle type inference automatically. It allows for creating both success and failure
/// results without explicitly specifying type parameters in most cases.
/// </remarks>
/// <example>
/// Creating a success result:
/// <code>
/// // Instead of:
/// var result1 = new Result&lt;Order&gt;(order, null);
/// 
/// // You can write:
/// var result2 = Result.Success(order);
/// </code>
/// 
/// Creating a failure result:
/// <code>
/// // With an error object:
/// var error = new ResultError("ORD001", "Invalid Order", "Order total cannot be negative");
/// var result1 = Result.Failure&lt;Order&gt;(error);
/// 
/// // Or with direct error details:
/// var result2 = Result.Failure&lt;Order&gt;("ORD001", "Invalid Order", "Order total cannot be negative");
/// </code>
/// </example>
public static class Result
{
    /// <summary>
    /// Creates a successful result without a specific value, using <see cref="Unit"/> as a placeholder.
    /// </summary>
    /// <returns>A new successful <see cref="Result{Unit}"/> instance.</returns>
    /// <remarks>
    /// This method is useful for operations that need to indicate success/failure but don't need to return a specific value.
    /// It uses the <see cref="Unit"/> type as a "void" equivalent in the Result pattern.
    /// </remarks>
    /// <example>
    /// Using Unit result for an operation without a return value:
    /// <code>
    /// public Result&lt;Unit&gt; DeleteUser(string userId)
    /// {
    ///     try
    ///     {
    ///         _repository.DeleteUser(userId);
    ///         return Result.Success(); // Returns Result&lt;Unit&gt;
    ///     }
    ///     catch (Exception ex)
    ///     {
    ///         return Result.Failure&lt;Unit&gt;("DELETE_FAILED", "Deletion Failed", ex.Message);
    ///     }
    /// }
    /// </code>
    /// 
    /// Using with async operations:
    /// <code>
    /// public async Task&lt;Result&lt;Unit&gt;&gt; SendEmailAsync(string to, string subject)
    /// {
    ///     try
    ///     {
    ///         await _emailService.SendAsync(to, subject);
    ///         return Result.Success(); // Returns Result&lt;Unit&gt;
    ///     }
    ///     catch (Exception ex)
    ///     {
    ///         return Result.Failure&lt;Unit&gt;("EMAIL_FAILED", "Send Failed", ex.Message);
    ///     }
    /// }
    /// </code>
    /// </example>
    public static Result<Unit> Success() => new(Unit.Value, null);

    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the result value. Must be a non-nullable type.</typeparam>
    /// <param name="value">The value to be contained in the successful result.</param>
    /// <returns>A new <see cref="Result{TValue}"/> instance representing a successful operation.</returns>
    /// <remarks>
    /// This method provides a convenient way to create a success result with automatic type inference,
    /// eliminating the need to explicitly specify the type parameter in most cases.
    /// </remarks>
    /// <example>
    /// <code>
    /// public Result&lt;User&gt; GetUser(string id)
    /// {
    ///     var user = _repository.FindUser(id);
    ///     return user != null
    ///         ? Result.Success(user)
    ///         : Result.Failure&lt;User&gt;("USER_NOT_FOUND", "Not Found", "User does not exist");
    /// }
    /// </code>
    /// </example>
    public static Result<TValue> Success<TValue>(TValue value) where TValue : notnull
        => new(value, null);

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <typeparam name="TValue">The type that would have been returned in case of success. Must be a non-nullable type.</typeparam>
    /// <param name="error">The error information describing the failure.</param>
    /// <returns>A new <see cref="Result{TValue}"/> instance representing a failed operation.</returns>
    /// <remarks>
    /// This method creates a failure result using a pre-constructed <see cref="ResultError"/> object,
    /// allowing for more detailed error information including custom error categories.
    /// </remarks>
    /// <example>
    /// <code>
    /// public Result&lt;Order&gt; ProcessOrder(OrderRequest request)
    /// {
    ///     if (!request.IsValid)
    ///     {
    ///         var error = new ResultError(
    ///             "INVALID_ORDER",
    ///             "Validation Failed",
    ///             "The order request contains invalid data");
    ///         return Result.Failure&lt;Order&gt;(error);
    ///     }
    ///     
    ///     // Process order...
    /// }
    /// </code>
    /// </example>
    public static Result<TValue> Failure<TValue>(ResultError error)
        where TValue : notnull => new(default, error);

    /// <summary>
    /// Creates a failed result with the specified error using a custom error category.
    /// </summary>
    /// <typeparam name="TValue">The type that would have been returned in case of success. Must be a non-nullable type.</typeparam>
    /// <typeparam name="TErrorCategory">The enum type used for categorizing errors. Must be a value type (struct) and an enum.</typeparam>
    /// <param name="error">The error information describing the failure, with custom error categorization.</param>
    /// <returns>A new <see cref="Result{TValue, TErrorCategory}"/> instance representing a failed operation.</returns>
    /// <remarks>
    /// <para>
    /// This method creates a failure result using a pre-constructed <see cref="ResultError{TErrorCategory}"/> object,
    /// allowing for domain-specific error categorization through the generic <typeparamref name="TErrorCategory"/> parameter.
    /// </para>
    /// <para>
    /// Use this overload when you need to work with custom error categories instead of the standard <see cref="ErrorCategory"/> enum.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// public enum OrderErrorCategory
    /// {
    ///     Validation,
    ///     Processing,
    ///     Payment
    /// }
    /// 
    /// public Result&lt;Order, OrderErrorCategory&gt; ProcessOrder(OrderRequest request)
    /// {
    ///     if (!request.IsValid)
    ///     {
    ///         var error = new ResultError&lt;OrderErrorCategory&gt;(
    ///             "INVALID_ORDER",
    ///             "Validation Failed",
    ///             "The order request contains invalid data",
    ///             OrderErrorCategory.Validation);
    ///         return Result.Failure&lt;Order, OrderErrorCategory&gt;(error);
    ///     }
    ///     
    ///     // Process order...
    /// }
    /// </code>
    /// </example>
    public static Result<TValue, TErrorCategory> Failure<TValue, TErrorCategory>(ResultError<TErrorCategory> error)
        where TValue : notnull where TErrorCategory : struct, Enum => new(default, error);

    /// <summary>
    /// Creates a failed result with the specified error details.
    /// </summary>
    /// <typeparam name="TValue">The type that would have been returned in case of success. Must be a non-nullable type.</typeparam>
    /// <param name="code">A unique identifier for the error type.</param>
    /// <param name="reason">A brief description of why the error occurred.</param>
    /// <param name="message">A detailed message explaining the error.</param>
    /// <returns>A new <see cref="Result{TValue}"/> instance representing a failed operation.</returns>
    /// <remarks>
    /// This method provides a convenient way to create a failure result when you don't need
    /// to specify an error category or create a separate <see cref="ResultError"/> instance.
    /// It uses the default <see cref="ErrorCategory"/> for error categorization.
    /// </remarks>
    /// <example>
    /// <code>
    /// public Result&lt;decimal&gt; CalculateDiscount(Order order)
    /// {
    ///     if (order.Total &lt; 0)
    ///     {
    ///         return Result.Failure&lt;decimal&gt;(
    ///             "INVALID_AMOUNT",
    ///             "Invalid Order Total",
    ///             "Order total cannot be negative");
    ///     }
    ///     
    ///     return Result.Success(order.Total * 0.1m);
    /// }
    /// </code>
    /// </example>
    public static Result<TValue> Failure<TValue>(string code, string reason, string message) where TValue : notnull
        => new(default, new ResultError(code, reason, message));
}