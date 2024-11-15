namespace ResultObject;

/// <summary>
/// Represents standard categories of errors that can occur in an application.
/// </summary>
/// <remarks>
/// This enumeration provides a standardized way to categorize errors across different parts of an application.
/// Each category represents a distinct class of errors, making it easier to handle and process errors appropriately
/// based on their type.
/// </remarks>
public enum ErrorCategory
{
    /// <summary>
    /// Indicates that the error occurred due to invalid input or business rule violations.
    /// </summary>
    /// <remarks>
    /// Use this category when:
    /// - Input data fails validation rules
    /// - Business constraints are violated
    /// - Data format is incorrect
    /// </remarks>
    Validation,

    /// <summary>
    /// Indicates that the requested resource could not be found.
    /// </summary>
    /// <remarks>
    /// Use this category when:
    /// - A requested entity doesn't exist
    /// - A specified path or route is invalid
    /// - A required resource is missing
    /// </remarks>
    NotFound,

    /// <summary>
    /// Indicates that authentication is required or has failed.
    /// </summary>
    /// <remarks>
    /// Use this category when:
    /// - User is not authenticated
    /// - Credentials are invalid or expired
    /// - Authentication token is missing
    /// </remarks>
    Unauthorized,

    /// <summary>
    /// Indicates that the authenticated user lacks necessary permissions.
    /// </summary>
    /// <remarks>
    /// Use this category when:
    /// - User is authenticated but lacks required permissions
    /// - Access to a resource is denied due to insufficient privileges
    /// - Operation is not allowed for the current user role
    /// </remarks>
    Forbidden,

    /// <summary>
    /// Indicates that the operation conflicts with the current state.
    /// </summary>
    /// <remarks>
    /// Use this category when:
    /// - Concurrent modification conflicts occur
    /// - Unique constraints are violated
    /// - Resource state prevents the requested operation
    /// </remarks>
    Conflict,

    /// <summary>
    /// Indicates an internal error in the application.
    /// </summary>
    /// <remarks>
    /// Use this category when:
    /// - Unexpected exceptions occur
    /// - System-level errors happen
    /// - Internal service failures occur
    /// 
    /// Note: These errors typically should not expose detailed information to external clients.
    /// </remarks>
    Internal,

    /// <summary>
    /// Indicates an error in an external system or service.
    /// </summary>
    /// <remarks>
    /// Use this category when:
    /// - External API calls fail
    /// - Third-party service errors occur
    /// - Network or communication errors happen with external systems
    /// </remarks>
    External
}