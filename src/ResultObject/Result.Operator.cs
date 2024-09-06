namespace ResultObject;

/// <summary>
/// Helper class encapsulating the implicit operators of the <c>Result</c> class.
/// </summary>
public partial class Result<T>
{
    /// <summary>
    /// Implicitly converts a successful result to its value,
    /// or returns the default value if the result is unsuccessful or the value is null.
    /// 
    /// <example>
    /// Usage example:
    /// 
    /// <code>
    /// int myInt = Result.Success(99);
    /// </code>
    /// 
    /// or
    /// 
    /// <code>
    /// var result = FunctionThatReturnsResult();
    /// if (result.IsFailure)
    /// {
    ///     // handle the result error...
    ///     // otherwise implicitly use the result value below.
    /// }
    /// MyType value = result;
    /// </code>
    /// 
    /// </example>
    /// </summary>
    /// <remarks>
    /// The original implementation of this operator threw an exception
    /// if the result was unsuccessful or the value was null.
    /// <br/>
    /// However, this approach was revised to return a nullable value instead,
    /// aligning better with the purpose of the Result class.
    /// </remarks>
    /// <param name="result">The result object to convert.</param>
    public static implicit operator T?(Result<T> result)
    {
        if (!result.IsSuccess || result.Value is null)
        {
            return default;
        }

        return result.Value;
    }

    /// <summary>
    /// Implicitly converts the value to a successful result.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value, null);
    }
}
