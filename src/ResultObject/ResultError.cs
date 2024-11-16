namespace ResultObject;

/// <summary>
/// Base record for error result implementations providing common functionality.
/// </summary>
public abstract record ResultErrorBase(
    string Code,
    string Reason,
    string Message,
    ResultErrorBase? InnerError,
    string? StackTrace
)
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
    /// Creates a new instance of the error with the current stack trace information.
    /// </summary>
    /// <returns>A new error instance with the current stack trace.</returns>
    public abstract ResultErrorBase WithStackTrace();

    /// <summary>
    /// Creates a sanitized version of the error suitable for external presentation.
    /// </summary>
    protected ResultErrorBase Sanitize(SanitizationLevel level) =>
        level switch
        {
            SanitizationLevel.None => this,
            SanitizationLevel.MessageOnly => this with
            {
                Message = "An error occurred.",
                StackTrace = null
            },
            SanitizationLevel.Full => this with
            {
                Message = "An error occurred.",
                Reason = "Internal Error",
                StackTrace = null,
                InnerError = null
            },
            _ => this
        };

    /// <summary>
    /// Returns a string representation of the error, including code, reason, and message.
    /// </summary>
    public override string ToString() =>
        $"Code: {Code}, Reason: {Reason}, Message: {Message}" +
        (StackTrace != null ? $"\nStack Trace: {StackTrace}" : string.Empty) +
        (InnerError != null ? $"\nInner Error: {InnerError}" : string.Empty);
}

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
    ResultErrorBase? InnerError = null,
    string? StackTrace = null) : ResultErrorBase(Code, Reason, Message, InnerError, StackTrace)
    where TErrorCategory : struct, Enum
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
    public override ResultError<TErrorCategory> WithStackTrace() => this with { StackTrace = Environment.StackTrace };

    /// <summary>
    /// Creates a sanitized version of the error suitable for external presentation.
    /// </summary>
    /// <param name="level">The desired level of sanitization to apply.</param>
    /// <returns>A new <see cref="ResultError{TErrorCategory}"/> instance with sanitized information.</returns>
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
    public new ResultError<TErrorCategory> Sanitize(SanitizationLevel level = SanitizationLevel.MessageOnly) =>
        (ResultError<TErrorCategory>)base.Sanitize(level);

    /// <summary>
    /// Returns a string representation of the error, including category, code, reason, and message.
    /// </summary>
    /// <returns>A formatted string containing the error details.</returns>
    /// <remarks>
    /// The string representation includes all critical error information and any inner error details.
    /// This is useful for logging and debugging purposes.
    /// </remarks>
    public override string ToString() => $"[{Category}] {base.ToString()}";
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
    ResultErrorBase? InnerError = null,
    string? StackTrace = null)
    : ResultError<ErrorCategory>(Code, Reason, Message, Category, InnerError, StackTrace)
{
    /// <summary>
    /// Creates a new instance of the error with the current stack trace information.
    /// </summary>
    /// <returns>A new <see cref="ResultError"/> instance with the current stack trace.</returns>
    /// <remarks>
    /// This method is useful for capturing the call stack at specific points during error handling.
    /// The stack trace can help with debugging and error tracking in development environments.
    /// </remarks>
    /// <example>
    /// <code>
    /// var error = new ResultError("CODE", "Reason", "Message")
    ///     .WithStackTrace();
    /// </code>
    /// </example>
    public new ResultError WithStackTrace() => (ResultError)base.WithStackTrace();

    /// <summary>
    /// Creates a sanitized version of the error suitable for external presentation.
    /// </summary>
    /// <param name="level">The desired level of sanitization to apply.</param>
    /// <returns>A new <see cref="ResultError"/> instance with sanitized information.</returns>
    /// <remarks>
    /// Sanitization helps prevent sensitive information leakage when errors are exposed to external systems or end users.
    /// Different levels of sanitization can be applied based on the security requirements:
    /// - None: No sanitization is applied
    /// - MessageOnly: Only the message is sanitized
    /// - Full: All potentially sensitive information is removed
    /// </remarks>
    /// <example>
    /// <code>
    /// var error = new ResultError(
    ///     "SENSITIVE_ERROR",
    ///     "Database Error",
    ///     "Failed to connect to db server: myserver:1433")
    ///     .Sanitize(SanitizationLevel.Full);
    /// // Results in a sanitized error with generic message
    /// </code>
    /// </example>
    public new ResultError Sanitize(SanitizationLevel level = SanitizationLevel.MessageOnly) =>
        (ResultError)base.Sanitize(level);

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