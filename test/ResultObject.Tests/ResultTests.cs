namespace ResultObject.Tests;

public class ResultTests
{
    [Fact]
    public void Success_WithValue_ShouldCreateSuccessResult()
    {
        // Arrange
        const string value = "test";

        // Act
        var result = Result.Success(value);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(value);
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Success_WithoutValue_ShouldCreateUnitResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Failure_WithError_ShouldCreateFailureResult()
    {
        // Arrange
        var error = new ResultError("CODE", "Reason", "Message");

        // Act
        var result = Result.Failure<string>(error);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Failure_WithErrorDetails_ShouldCreateFailureResult()
    {
        // Arrange
        const string code = "CODE";
        const string reason = "Reason";
        const string message = "Message";

        // Act
        var result = Result.Failure<string>(code, reason, message);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be(code);
        result.Error.Reason.Should().Be(reason);
        result.Error.Message.Should().Be(message);
    }
}

public class ResultGenericTests
{
    [Fact]
    public void Cast_FromSuccessResult_ShouldReturnCastSuccessResult()
    {
        // Arrange
        const int value = 42;
        var result = Result.Success(value);

        // Act
        var castResult = result.Cast<object>();

        // Assert
        castResult.Should().NotBeNull();
        castResult.IsSuccess.Should().BeTrue();
        castResult.Value.Should().Be(value);
    }

    [Fact]
    public void Cast_FromFailureResult_ShouldReturnFailureResult()
    {
        // Arrange
        var error = new ResultError("CODE", "Reason", "Message");
        var result = Result.Failure<int>(error);

        // Act
        var castResult = result.Cast<string>();

        // Assert
        castResult.Should().NotBeNull();
        castResult.IsFailure.Should().BeTrue();
        castResult.Error.Should().Be(error);
    }

    [Fact]
    public void Cast_WithInvalidCast_ShouldThrowInvalidCastException()
    {
        // Arrange
        const string value = "test";
        var result = Result.Success(value);

        // Act & Assert
        result.Invoking(r => r.Cast<int>())
            .Should().Throw<InvalidCastException>();
    }

    [Fact]
    public void Constructor_WithNullValue_ShouldCreateFailureResult()
    {
        // Act
        var result = new Result<string>(null, null);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
    }
}

public class ResultErrorTests
{
    [Fact]
    public void Constructor_WithBasicProperties_ShouldCreateError()
    {
        // Arrange
        const string code = "CODE";
        const string reason = "Reason";
        const string message = "Message";
        var category = ErrorCategory.Validation;

        // Act
        var error = new ResultError(code, reason, message, category);

        // Assert
        error.Should().NotBeNull();
        error.Code.Should().Be(code);
        error.Reason.Should().Be(reason);
        error.Message.Should().Be(message);
        error.Category.Should().Be(category);
        error.InnerError.Should().BeNull();
        error.StackTrace.Should().BeNull();
    }

    [Fact]
    public void WithStackTrace_ShouldCreateNewInstanceWithStackTrace()
    {
        // Arrange
        var error = new ResultError("CODE", "Reason", "Message");

        // Act
        var errorWithStack = error.WithStackTrace();

        // Assert
        errorWithStack.Should().NotBe(error);
        errorWithStack.StackTrace.Should().NotBeNull();
        errorWithStack.StackTrace.Should().Contain("at ");
    }

    [Theory]
    [InlineData(ResultErrorBase.SanitizationLevel.None)]
    [InlineData(ResultErrorBase.SanitizationLevel.MessageOnly)]
    [InlineData(ResultErrorBase.SanitizationLevel.Full)]
    public void Sanitize_WithDifferentLevels_ShouldSanitizeAppropriately(ResultErrorBase.SanitizationLevel level)
    {
        // Arrange
        var error = new ResultError(
                "SENSITIVE_CODE",
                "Sensitive Reason",
                "Sensitive Message",
                ErrorCategory.Internal)
            .WithStackTrace();

        // Act
        var sanitized = error.Sanitize(level);

        // Assert
        switch (level)
        {
            case ResultErrorBase.SanitizationLevel.None:
                sanitized.Should().BeEquivalentTo(error);
                break;

            case ResultErrorBase.SanitizationLevel.MessageOnly:
                sanitized.Code.Should().Be(error.Code);
                sanitized.Reason.Should().Be(error.Reason);
                sanitized.Message.Should().Be("An error occurred.");
                sanitized.StackTrace.Should().BeNull();
                break;

            case ResultErrorBase.SanitizationLevel.Full:
                sanitized.Code.Should().Be(error.Code);
                sanitized.Reason.Should().Be("Internal Error");
                sanitized.Message.Should().Be("An error occurred.");
                sanitized.StackTrace.Should().BeNull();
                sanitized.InnerError.Should().BeNull();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }

    [Fact]
    public void ToString_WithFullError_ShouldIncludeAllComponents()
    {
        // Arrange
        var innerError = new ResultError("INNER", "Inner Reason", "Inner Message");
        var error = new ResultError(
                "CODE",
                "Reason",
                "Message",
                ErrorCategory.Validation,
                innerError)
            .WithStackTrace();

        // Act
        var toString = error.ToString();

        // Assert
        toString.Should().Contain("[Validation]");
        toString.Should().Contain("Code: CODE");
        toString.Should().Contain("Reason: Reason");
        toString.Should().Contain("Message: Message");
        toString.Should().Contain("Stack Trace:");
        toString.Should().Contain("Inner Error:");
        toString.Should().Contain("INNER");
    }
}

public class UnitTests
{
    [Fact]
    public void Value_ShouldBeSingleton()
    {
        // Arrange
        var value1 = Unit.Value;
        var value2 = Unit.Value;

        // Assert
        value1.Should().Be(value2);
    }

    [Fact]
    public void EqualityComparison_ShouldAlwaysBeTrue()
    {
        // Arrange
        var unit1 = new Unit();
        var unit2 = new Unit();

        // Assert
        unit1.Should().Be(unit2);
        unit1.Equals(unit2).Should().BeTrue();
    }
}

[Collection("Result Integration Tests")]
public class ResultIntegrationTests
{
    [Fact]
    public void ComplexScenario_WithChainedOperations_ShouldHandleErrorsProperly()
    {
        // Arrange
        var initialResult = Result.Success(42);

        // Act
        var finalResult = initialResult
            .Cast<object>()
            .Cast<int>()
            .Cast<IComparable>();

        // Assert
        finalResult.Should().NotBeNull();
        finalResult.IsSuccess.Should().BeTrue();
        finalResult.Value.Should().Be(42);
    }

    [Fact]
    public void ComplexScenario_WithErrorHandling_ShouldPreserveErrorInformation()
    {
        // Arrange
        var error = new ResultError(
            "COMPLEX_ERROR",
            "Complex Operation Failed",
            "Multiple steps failed",
            ErrorCategory.Internal,
            new ResultError("INNER", "Inner Failure", "Step 2 failed"));

        var result = Result.Failure<int>(error);

        // Act
        var sanitizedResult = Result.Failure<string, ErrorCategory>(
            result.Error!.Sanitize(ResultErrorBase.SanitizationLevel.Full)
        );

        // Assert
        sanitizedResult.Error.Should().NotBeNull();
        sanitizedResult.Error!.Message.Should().Be("An error occurred.");
        sanitizedResult.Error.Reason.Should().Be("Internal Error");
        sanitizedResult.Error.InnerError.Should().BeNull();
    }
}

[Collection("Error Category Tests")]
public class ErrorCategoryTests
{
    [Theory]
    [InlineData(ErrorCategory.Validation)]
    [InlineData(ErrorCategory.NotFound)]
    [InlineData(ErrorCategory.Unauthorized)]
    [InlineData(ErrorCategory.Forbidden)]
    [InlineData(ErrorCategory.Conflict)]
    [InlineData(ErrorCategory.Internal)]
    [InlineData(ErrorCategory.External)]
    public void ErrorCategory_ShouldBeUsableInResults(ErrorCategory category)
    {
        // Arrange & Act
        var error = new ResultError(
            "TEST",
            "Test Reason",
            "Test Message",
            category);

        // Assert
        error.Category.Should().Be(category);
    }
}