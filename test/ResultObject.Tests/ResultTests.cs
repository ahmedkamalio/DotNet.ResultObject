namespace ResultObject.Tests;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        const int value = 42;

        var result = Result.Success(value);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult_WithError()
    {
        var error = new ResultError("HTTP_500", "InternalError", "Something went wrong");

        var result = Result.Failure<object>(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult_WithErrorDetails()
    {
        const string code = "HTTP_404";
        const string reason = "NotFound";
        const string message = "The item was not found";

        var result = Result.Failure<int>(code, reason, message);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(default, result.Value);
        Assert.Equal(code, result.Error?.Code);
        Assert.Equal(reason, result.Error?.Reason);
        Assert.Equal(message, result.Error?.Message);
    }

    [Fact]
    public void ImplicitConversionToValue_ShouldReturnCorrectValue_ForSuccessfulResult()
    {
        const int value = 99;
        var result = Result.Success(value);

        int actualValue = result;

        Assert.Equal(value, actualValue);
    }

    [Fact]
    public void ImplicitConversionToValue_ShouldReturnDefaultValue_ForFailedResult()
    {
        var error = new ResultError("HTTP_500", "InternalError", "Something went wrong");
        int result = Result.Failure<int>(error);

        Assert.Equal(default, result);
    }

    [Fact]
    public void ImplicitConversionFromValue_ShouldCreateSuccessfulResult()
    {
        const int value = 77;

        Result<int> result = value;

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }
}
