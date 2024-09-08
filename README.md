# ResultObject

<img src="https://github.com/ahmedkamalio/DotNet.ResultObject/blob/main/assets/icon.png" alt="ResultObject Icon" width="100" height="100">

## Overview

The **ResultObject** package provides a utility to return errors instead of throwing exceptions, making it a generic and
easy-to-use mechanism for handling the outcome of operations. It encapsulates either a success with a value or a failure
with error information, promoting cleaner code by eliminating the need for exception-based error handling and offering a
clear distinction between success and failure scenarios.

## Features

- **Success/Failure Handling**: Easily handle success or failure outcomes of operations.
- **Generic Result Type**: Support for results containing any type of value.
- **Error Handling**: Encapsulate detailed error information on failure.
- **Type-Safe Failures**: Transform failure results into a different value type while preserving error information.
- **Implicit Operators**: Simplify result handling with implicit conversions between results and values.
- **Strict Value Enforcement**: Use `MustGetValue()` to enforce non-null results in critical operations.

## Installation

You can install the **ResultObject** package via NuGet. To do this, use the following command in your project.

DotNet CLI:

```bash
dotnet add package ResultObject
```

Alternatively, you can add it to your project using the Visual Studio package manager UI by searching for "
ResultObject."

## Usage

### Basic Result Example

The `Result<T>` class represents the outcome of an operation. It can either be a success (containing a value) or a
failure (containing error details).

```csharp
var successResult = Result.Success(42);

var failureResult = Result.Failure<int>(new ResultError("404", "Not Found", "The requested resource was not found."));
// or
var failureResult = Result.Failure<int>("404", "Not Found", "The requested resource was not found.");
```

### Checking Success or Failure

You can check whether an operation was successful or failed using the `IsSuccess` and `IsFailure` properties.

```csharp
if (successResult.IsSuccess)
{
    Console.WriteLine("Operation succeeded with value: " + successResult.Value);
}

if (failureResult.IsFailure)
{
    Console.WriteLine($"Operation failed with error: {failureResult.Error.Message}");
}
```

### Implicit Conversions

The `Result<T>` class supports implicit conversions between results and values for convenience. The implicit conversion
returns the value if the result is successful, or `default(T)` if the result is a failure or the value is `null`.

```csharp
int myValue = Result.Success(100);  // Implicit conversion from result to value.
Result<int> result = 200;  // Implicit conversion from value to result.
```

If you need stricter control over `null` values, you can use the `MustGetValue()` method.

### Enforcing Non-Null Results

In critical scenarios where you need to ensure that the result is non-null, you can use the `MustGetValue()` method.
This method throws an `InvalidOperationException` if the result is unsuccessful or the value is `null`.

```csharp
try
{
    var result = FunctionThatReturnsResult();
    int value = result.MustGetValue();  // Throws if result is failure or value is null.
    Console.WriteLine("Value: " + value);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
```

### Handling Errors

The `ResultError` class encapsulates details about the failure. This includes an error code, a reason, and a message.

```csharp
var error = new ResultError("500", "Server Error", "An unexpected error occurred.");
var failedResult = Result.Failure<int>(error);
```

## API Reference

### `Result<T>`

Represents the result of an operation with the following properties:

- `IsSuccess`: Indicates if the operation succeeded.
- `IsFailure`: Indicates if the operation failed.
- `Value`: The result value if the operation succeeded (or `null` if it failed).
- `Error`: Contains error details if the operation failed (or `null` if it succeeded).

#### Methods

- `ToFailureResult<TValue>()`: Converts a failure result to a result with a different value type while preserving error
  details.
- `MustGetValue()`: Throws an `InvalidOperationException` if the result is a failure or the value is `null`.

### `Result`

Helper class to create `Result<T>` instances:

- `Success()`: Creates an empty success result, this can be used to represent a successful operation that doesn't return
  a value.
- `Success<T>(T value)`: Creates a success result with value.
- `Failure<T>(ResultError error)`: Creates a failure result with error details.
- `Failure<T>(string code, string reason, string message)`: Shorthand to create a failure result with error details.

### `ResultError`

Represents an error with the following properties:

- `Code`: A string representing the error code.
- `Reason`: A brief reason for the error.
- `Message`: A detailed message explaining the error.

## License

This project is licensed under the MIT License.
