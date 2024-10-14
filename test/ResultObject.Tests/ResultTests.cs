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
    public void SuccessResult_Error_ShouldReturnNull()
    {
        // Arrange
        var result = Result.Success("Test Value");

        // Act
        var error = result.Error;

        // Assert
        Assert.Null(error);
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
    public void FailureResult_Value_ShouldReturnNull()
    {
        // Arrange
        var error = new ResultError("404", "NotFound", "The item was not found.");
        var result = Result.Failure<string>(error);

        // Act
        var value = result.Value;

        // Assert
        Assert.Null(value);
    }

    [Fact]
    public void FailureResult_Error_ShouldReturnCorrectError()
    {
        // Arrange
        var error = new ResultError("404", "NotFound", "The item was not found.");
        var result = Result.Failure<string>(error);

        // Act
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Result expected to be failure.");
        }

        // Assert
        Assert.Equal("404", result.Error.Code);
        Assert.Equal("NotFound", result.Error.Reason);
        Assert.Equal("The item was not found.", result.Error.Message);
    }

    [Fact]
    public void FailureResult_WithErrorCode_ShouldReturnCorrectError()
    {
        // Arrange
        var result = Result.Failure<string>("500", "InternalError", "An unexpected error occurred.");

        // Act
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Result expected to be failure.");
        }

        // Assert
        Assert.Equal("500", result.Error.Code);
        Assert.Equal("InternalError", result.Error.Reason);
        Assert.Equal("An unexpected error occurred.", result.Error.Message);
    }

    [Fact]
    public void CastResult_SuccessfulCast_ShouldReturnNewResult()
    {
        // Arrange
        var result = Result.Success<object>("Test Value");

        // Act
        var castResult = result.Cast<string>();

        // Assert
        Assert.True(castResult.IsSuccess);
        Assert.Equal("Test Value", castResult.Value);
    }

    [Fact]
    public void CastResult_Failure_ShouldPreserveError()
    {
        // Arrange
        var error = new ResultError("404", "NotFound", "The item was not found.");
        var result = Result.Failure<object>(error);

        // Act
        var castResult = result.Cast<string>();

        // Assert
        Assert.True(castResult.IsFailure);
        Assert.Equal(error, castResult.Error);
        Assert.Null(castResult.Value);
    }

    [Fact]
    public void CastResult_InvalidCast_ShouldThrowException()
    {
        // Arrange
        var result = Result.Success<object>(42);

        // Act & Assert
        Assert.Throws<InvalidCastException>(() => result.Cast<string>());
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
    public void SuccessResult_WithNullValue_ShouldHaveIsSuccessFalse()
    {
        // Arrange
        var result = Result.Success<string?>(null);

        // Act & Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
    }
}