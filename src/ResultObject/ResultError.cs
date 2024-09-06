namespace ResultObject;

/// <summary>
/// Represents the details of an result error.
/// </summary>
public record ResultError(string Code, string Reason, string Message);
