namespace ResultObject.Tests;

public class ResultTests
{
    [Fact]
    public void SuccessResult_IsSuccess_ShouldBeTrue()
    {
        // Arrange
        var result = Result.Success("Test Value");

        // Act & Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void SuccessResult_Value_ShouldReturnCorrectValue()
    {
        // Arrange
        var result = Result.Success("Test Value");

        // Act
        var value = result.Value;

        // Assert
        Assert.Equal("Test Value", value);
    }

    [Fact]
    public void SuccessResult_ValueOrDefault_ShouldReturnCorrectValue()
    {
        // Arrange
        var result = Result.Success("Test Value");

        // Act
        var valueOrDefault = result.ValueOrDefault;

        // Assert
        Assert.Equal("Test Value", valueOrDefault);
    }

    [Fact]
    public void SuccessResult_Error_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var result = Result.Success("Test Value");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => result.Error);
    }

    [Fact]
    public void SuccessResult_ErrorOrDefault_ShouldReturnNull()
    {
        // Arrange
        var result = Result.Success("Test Value");

        // Act
        var errorOrDefault = result.ErrorOrDefault;

        // Assert
        Assert.Null(errorOrDefault);
    }

    [Fact]
    public void FailureResult_IsFailure_ShouldBeTrue()
    {
        // Arrange
        var error = new ResultError("404", "NotFound", "The item was not found.");
        var result = Result.Failure<string>(error);

        // Act & Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void FailureResult_Value_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var error = new ResultError("404", "NotFound", "The item was not found.");
        var result = Result.Failure<string>(error);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void FailureResult_ValueOrDefault_ShouldReturnDefaultValue()
    {
        // Arrange
        var error = new ResultError("404", "NotFound", "The item was not found.");
        var result = Result.Failure<string>(error);

        // Act
        var valueOrDefault = result.ValueOrDefault;

        // Assert
        Assert.Null(valueOrDefault);
    }

    [Fact]
    public void FailureResult_Error_ShouldReturnCorrectError()
    {
        // Arrange
        var error = new ResultError("404", "NotFound", "The item was not found.");
        var result = Result.Failure<string>(error);

        // Act
        var resultError = result.Error;

        // Assert
        Assert.Equal("404", resultError.Code);
        Assert.Equal("NotFound", resultError.Reason);
        Assert.Equal("The item was not found.", resultError.Message);
    }

    [Fact]
    public void FailureResult_ErrorOrDefault_ShouldReturnCorrectError()
    {
        // Arrange
        var error = new ResultError("404", "NotFound", "The item was not found.");
        var result = Result.Failure<string>(error);

        // Act
        var errorOrDefault = result.ErrorOrDefault;

        // Assert
        Assert.Equal(error, errorOrDefault);
    }

    [Fact]
    public void FailureResult_WithErrorCode_ShouldReturnCorrectError()
    {
        // Arrange
        var result = Result.Failure<string>("500", "InternalError", "An unexpected error occurred.");

        // Act
        var error = result.Error;

        // Assert
        Assert.Equal("500", error.Code);
        Assert.Equal("InternalError", error.Reason);
        Assert.Equal("An unexpected error occurred.", error.Message);
    }

    [Fact]
    public void ToFailureResult_ShouldConvertToNewFailureResult()
    {
        // Arrange
        var error = new ResultError("404", "NotFound", "The item was not found.");
        var result = Result.Failure<string>(error);

        // Act
        var newResult = result.ToFailureResult<int>();

        // Assert
        Assert.True(newResult.IsFailure);
        Assert.Equal(error, newResult.Error);
        Assert.Equal(default, newResult.ValueOrDefault);
    }

    [Fact]
    public void ResultError_ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var error = new ResultError("500", "InternalError", "An unexpected error occurred.");

        // Act
        var resultString = error.ToString();

        // Assert
        Assert.Equal("Code: 500, Reason: InternalError, Message: An unexpected error occurred.", resultString);
    }

    [Fact]
    public void SuccessResult_WithNullValue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var result = Result.Success<string?>(null);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}