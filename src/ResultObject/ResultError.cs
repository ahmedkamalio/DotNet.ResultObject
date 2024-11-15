namespace ResultObject;

/// <summary>
/// Represents a strongly-typed error result with a customizable error category.
/// </summary>
/// <typeparam name="TErrorCategory">The enum type representing error categories.</typeparam>
/// <param name="Code">A unique identifier for the error type.</param>
/// <param name="Reason">A brief description of why the error occurred.</param>
/// <param name="Message">A detailed message explaining the error.</param>
/// <param name="Category">The category of the error, used for classification and handling.</param>
/// <param name="InnerError">An optional nested error that provides additional context.</param>
/// <param name="StackTrace">An optional stack trace for debugging purposes.</param>
/// <remarks>
/// This record provides a structured way to represent errors with categorization.
/// The generic type parameter must be an enum, allowing for domain-specific error categorization.
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
/// var error = new ResultError&lt;OrderErrorCategory&gt;(
///     "ORD001",
///     "Invalid Order",
///     "The order total cannot be negative",
///     OrderErrorCategory.Validation);
/// </code>
/// </example>
public record ResultError<TErrorCategory>(
    string Code,
    string Reason,
    string Message,
    TErrorCategory? Category = null,
    ResultError? InnerError = null,
    string? StackTrace = null
) where TErrorCategory : struct, Enum
{
    /// <summary>
    /// Creates a new instance of the error with the current stack trace information.
    /// </summary>
    /// <returns>A new <see cref="ResultError{TErrorCategory}"/> instance with the current stack trace.</returns>
    /// <remarks>
    /// This method is useful for capturing the call stack at specific points during error handling.
    /// The stack trace can help with debugging and error tracking in development environments.
    /// </remarks>
    /// <example>
    /// <code>
    /// var error = new ResultError&lt;ErrorCategory&gt;("CODE", "Reason", "Message")
    ///     .WithStackTrace();
    /// </code>
    /// </example>
    public ResultError<TErrorCategory> WithStackTrace() =>
        new(Code, Reason, Message, Category, InnerError, Environment.StackTrace);

    /// <summary>
    /// Creates a sanitized version of the error suitable for external presentation.
    /// </summary>
    /// <param name="level">The desired level of sanitization to apply.</param>
    /// <returns>A new <see cref="ResultError{TErrorCategory}"/> instance with sanitized information.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid sanitization level is provided.</exception>
    /// <remarks>
    /// Sanitization helps prevent sensitive information leakage when errors are exposed to external systems or end users.
    /// Different levels of sanitization can be applied based on the security requirements:
    /// - None: No sanitization is applied
    /// - MessageOnly: Only the message is sanitized
    /// - Full: All potentially sensitive information is removed
    /// </remarks>
    /// <example>
    /// <code>
    /// var error = new ResultError&lt;ErrorCategory&gt;(
    ///     "SENSITIVE_ERROR",
    ///     "Database Error",
    ///     "Failed to connect to db server: myserver:1433")
    ///     .Sanitize(SanitizationLevel.Full);
    /// // Results in a sanitized error with generic message
    /// </code>
    /// </example>
    public ResultError<TErrorCategory> Sanitize(
        ResultError.SanitizationLevel level = ResultError.SanitizationLevel.MessageOnly)
    {
        return level switch
        {
            ResultError.SanitizationLevel.None => this,
            ResultError.SanitizationLevel.MessageOnly => this with
            {
                Message = "An error occurred.",
                StackTrace = null
            },
            ResultError.SanitizationLevel.Full => this with
            {
                Message = "An error occurred.",
                Reason = "Internal Error",
                StackTrace = null,
                InnerError = null
            },
            _ => throw new ArgumentException($"Invalid sanitization level: {level}")
        };
    }

    /// <summary>
    /// Returns a string representation of the error, including category, code, reason, and message.
    /// </summary>
    /// <returns>A formatted string containing the error details.</returns>
    /// <remarks>
    /// The string representation includes all critical error information and any inner error details.
    /// This is useful for logging and debugging purposes.
    /// </remarks>
    public override string ToString() =>
        $"[{Category}] Code: {Code}, Reason: {Reason}, Message: {Message}" +
        (StackTrace != null ? $"\nStack Trace: {StackTrace}" : string.Empty) +
        (InnerError != null ? $"\nInner Error: {InnerError}" : string.Empty);
}

/// <summary>
/// Represents a pre-configured error result using the standard <see cref="ErrorCategory"/> enumeration.
/// </summary>
/// <param name="Code">A unique identifier for the error type.</param>
/// <param name="Reason">A brief description of why the error occurred.</param>
/// <param name="Message">A detailed message explaining the error.</param>
/// <param name="Category">The category of the error from the standard <see cref="ErrorCategory"/> enum.</param>
/// <param name="InnerError">An optional nested error that provides additional context.</param>
/// <param name="StackTrace">An optional stack trace for debugging purposes.</param>
/// <remarks>
/// This record provides a concrete implementation of <see cref="ResultError{TErrorCategory}"/>
/// using the standard <see cref="ErrorCategory"/> enumeration. It's designed for general-purpose
/// error handling scenarios where custom categorization is not required.
/// </remarks>
/// <example>
/// <code>
/// var error = new ResultError(
///     "AUTH001",
///     "Authentication Failed",
///     "Invalid credentials provided",
///     ErrorCategory.Unauthorized);
/// </code>
/// </example>
public record ResultError(
    string Code,
    string Reason,
    string Message,
    ErrorCategory? Category = null,
    ResultError? InnerError = null,
    string? StackTrace = null)
    : ResultError<ErrorCategory>(Code, Reason, Message, Category, InnerError, StackTrace)
{
    /// <summary>
    /// Defines the level of detail to include when sanitizing error information.
    /// </summary>
    public enum SanitizationLevel
    {
        /// <summary>
        /// No information is removed or modified.
        /// </summary>
        None,

        /// <summary>
        /// Only sensitive message content is sanitized while preserving error structure.
        /// </summary>
        MessageOnly,

        /// <summary>
        /// All potentially sensitive information is removed, including stack traces and inner errors.
        /// </summary>
        Full
    }

    /// <summary>
    /// Returns a string representation of the error, including category, code, reason, and message.
    /// </summary>
    /// <returns>A formatted string containing the error details.</returns>
    /// <remarks>
    /// The string representation includes all critical error information and any inner error details.
    /// This is useful for logging and debugging purposes.
    /// </remarks>
    public override string ToString() => base.ToString();
}