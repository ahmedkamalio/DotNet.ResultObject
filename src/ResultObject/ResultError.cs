namespace ResultObject;

/// <summary>
/// Represents the details of a result error, including an error code, a reason for the failure, and a detailed message.
/// </summary>
public record ResultError(string Code, string Reason, string Message)
{
    /// <summary>
    /// Returns a string that represents the current error, providing a summary with the error code, reason, and message.
    /// </summary>
    /// <returns>
    /// A string that contains the error code, reason, and message for this <see cref="ResultError"/>.
    /// </returns>
    public override string ToString()
    {
        return $"Code: {Code}, Reason: {Reason}, Message: {Message}";
    }
}