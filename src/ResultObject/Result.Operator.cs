namespace ResultObject;

/// <summary>
/// Helper class encapsulating the implicit operators of the <c>Result</c> class.
/// </summary>
public partial class Result<T>
{
    /// <summary>
    /// Implicitly converts a <see cref="Result{T}"/> to its value.
    /// </summary>
    /// <remarks>
    /// This conversion will return the value if the result is successful and the value is not null.
    /// If the result is unsuccessful or the value is null, it will return the default value for the type <typeparamref name="T"/> (e.g., <c>null</c> for reference types, <c>0</c> for integers).
    /// <br />
    /// This is useful in scenarios where you want a convenient way to retrieve the result's value without explicit null-checking or error handling, such as non-critical operations where a default value is acceptable.
    /// </remarks>
    /// <param name="result">The result object to convert.</param>
    /// <returns>The value of the result if successful and not null; otherwise, the default value of <typeparamref name="T"/>.</returns>
    public static implicit operator T?(Result<T> result) =>
        result.IsFailure || result.Value is null ? default : result.Value;

    /// <summary>
    /// Implicitly converts the value to a successful result.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Result<T>(T value) => new(value, null);
}