# ResultObject

<img src="https://raw.githubusercontent.com/ahmedkamalio/DotNet.ResultObject/main/icon.png" alt="ResultObject Icon" width="100" height="100">

## Overview

The **ResultObject** package provides a robust utility to handle operation outcomes in .NET applications. It promotes
clean code by clearly separating success from failure scenarios, offering rich error categorization, and providing
type-safe error handling without relying on exceptions.

## Features

- **Success/Failure Handling**: Type-safe operation results with clear success/failure states
- **Error Categories**: Built-in and custom error categorization for better error handling
- **Framework Support**: Targets both .NET 8.0 and .NET 9.0
- **Generic Result Types**: Type-safe results with support for any value type
- **Error Management**: Detailed error information with categorization and sanitization
- **Type-Safe Casting**: Transform result values while preserving error information
- **Unit Results**: Support for void-equivalent operations
- **Stack Trace Support**: Optional stack traces for debugging
- **Error Sanitization**: Configurable error detail sanitization for external exposure

## Installation

Install via NuGet:

```bash
dotnet add package ResultObject
```

Or search for "ResultObject" in the Visual Studio package manager.

## Basic Usage

### Creating Results

```csharp
// Success results
var successResult = Result.Success(42);
var voidSuccess = Result.Success(); // Returns Result<Unit>

// Failure results
var failureResult = Result.Failure<int>(
    "404", 
    "Not Found", 
    "The requested resource was not found."
);
```

### Checking Results

```csharp
if (successResult.IsSuccess)
{
    Console.WriteLine($"Operation succeeded with value: {successResult.Value}");
}

if (failureResult.IsFailure)
{
    Console.WriteLine($"Operation failed: {failureResult.Error.Message}");
}
```

### Using Error Categories

```csharp
var error = new ResultError(
    "USER_404",
    "User Not Found",
    "The specified user does not exist",
    ErrorCategory.NotFound
);

var result = Result.Failure<User>(error);

if (result.Error?.Category == ErrorCategory.NotFound)
{
    // Handle not found case
}
```

### Custom Error Categories

```csharp
public enum OrderErrorCategory
{
    Validation,
    Processing,
    Payment
}

var error = new ResultError<OrderErrorCategory>(
    "ORD001",
    "Invalid Order",
    "Order total cannot be negative",
    OrderErrorCategory.Validation
);

var result = Result.Failure<Order, OrderErrorCategory>(error);
```

### Error Sanitization

```csharp
var error = new ResultError(
    "DB_ERROR",
    "Database Connection Failed",
    "Failed to connect to server: sensitive-server:1433"
)
.WithStackTrace();

// Sanitize for external use
var sanitized = error.Sanitize(ResultError.SanitizationLevel.Full);
```

### Type Casting

```csharp
var result = Result.Success<object>("42");
var castResult = result.Cast<string>();

if (castResult.IsSuccess)
{
    Console.WriteLine($"Cast successful: {castResult.Value}");
}
```

### Unit Results (Void Operations)

```csharp
public Result<Unit> DeleteUser(string userId)
{
    try
    {
        // Delete user logic
        return Result.Success();
    }
    catch (Exception ex)
    {
        return Result.Failure<Unit>("DELETE_FAILED", "Deletion Failed", ex.Message);
    }
}
```

## API Reference

### `IResult<TValue, TErrorCategory>`

- `IsSuccess`: Indicates if the operation succeeded
- `IsFailure`: Indicates if the operation failed
- `Value`: The operation's value (if successful)
- `Error`: Error information (if failed)
- `Cast<T>()`: Safely cast the result's value

### `Result` Static Class

- `Success()`: Creates a Unit result
- `Success<TValue>(TValue value)`: Creates a success result
- `Failure<TValue>(ResultError error)`: Creates a failure result
- `Failure<TValue>(string code, string reason, string message)`: Creates a failure result
- `Failure<TValue, TErrorCategory>(ResultError<TErrorCategory> error)`: Creates a categorized failure result

### `ResultError<TErrorCategory>`

- `Code`: Error identifier
- `Reason`: Brief error description
- `Message`: Detailed error message
- `Category`: Error category
- `InnerError`: Nested error information
- `StackTrace`: Optional stack trace
- `WithStackTrace()`: Adds current stack trace
- `Sanitize()`: Creates sanitized error copy

### `ErrorCategory` (Built-in)

- `Validation`: Input/business rule violations
- `NotFound`: Resource not found
- `Unauthorized`: Authentication required/failed
- `Forbidden`: Insufficient permissions
- `Conflict`: State conflicts
- `Internal`: System errors
- `External`: External service errors

## License

This project is licensed under the MIT License.
