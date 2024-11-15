namespace ResultObject;

/// <summary>
/// Represents a void type, used when no value needs to be returned but a Result type is required.
/// </summary>
public readonly record struct Unit
{
    /// <summary>
    /// Gets the singleton instance of Unit.
    /// </summary>
    public static readonly Unit Value = new();
}