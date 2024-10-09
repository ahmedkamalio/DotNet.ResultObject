# ResultObject

<img src="https://raw.githubusercontent.com/ahmedkamalio/DotNet.ResultObject/main/icon.png" alt="ResultObject Icon" width="100" height="100">

## Overview

The **ResultObject** package provides a utility to return errors instead of throwing exceptions, making it a generic and
easy-to-use mechanism for handling the outcome of operations. It encapsulates either a success with a value or a failure
with error information, promoting cleaner code by eliminating the need for exception-based error handling and offering a
clear distinction between success and failure scenarios.

## Features

- **Success/Failure Handling**: Easily handle success or failure outcomes of operations.
- **Generic Result Type**: Support for results containing any type of value.
- **Error Handling**: Encapsulate detailed error information on failure.
- **Type-Safe Failures**: Convert failure results into different value types while preserving error information.
- **Default Value Handling**: Use `ValueOrDefault` to safely retrieve the value or a default when the result is a
  failure.
- **Strict Value Enforcement**: Use the `Value` property to enforce non-null results, throwing an exception on failure.

## Installation

You can install the **ResultObject** package via NuGet. Use the following command in your project.

DotNet CLI:

```bash
dotnet add package ResultObject
```

Alternatively, you can add it to your project using the Visual Studio package manager UI by searching for "
ResultObject."

## Usage

### Basic Result Example

The `Result<TValue>` class represents the outcome of an operation. It can either be a success (containing a value) or a
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

### Retrieving Values Safely

The `Value` property gives you the result value if the operation succeeded, but throws an exception if accessed on a
failure. If you want to avoid exceptions, you can use the `ValueOrDefault` property to get the value or `null`/default.

```csharp
var value = successResult.ValueOrDefault;  // Retrieves value or default if failed.
```

### Enforcing Non-Null Results

The `Value` property enforces that the result must be successful and non-null. Accessing it on failure or null value
throws an `InvalidOperationException`.

```csharp
try
{
    var result = FunctionThatReturnsResult();
    int value = result.Value;  // Throws if result is failure or value is null.
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

### Converting Failure to a Different Value Type

You can convert a failure result to a result of a different value type while keeping the error information using
`ToFailureResult<T>()`.

```csharp
var failureResult = Result.Failure<int>("500", "Server Error", "An unexpected error occurred.");
var convertedFailure = failureResult.ToFailureResult<string>();  // Failure with string type.
```

## API Reference

### `Result<TValue>`

Represents the result of an operation with the following properties:

- `IsSuccess`: Indicates if the operation succeeded.
- `IsFailure`: Indicates if the operation failed.
- `Value`: The result value if the operation succeeded (or throws an exception if it failed).
- `ValueOrDefault`: The result value if the operation succeeded, or `default(TValue)` if it failed.
- `Error`: Contains error details if the operation failed (or throws an exception if it succeeded).
- `ErrorOrDefault`: The error details if the operation failed, or `null` if it succeeded.

#### Methods

- `ToFailureResult<T>()`: Converts a failure result to a result with a different value type while preserving error
  details.

### `Result`

Helper class to create `Result<TValue>` instances:

- `Success<TValue>(TValue value)`: Creates a success result with a value.
- `Failure<TValue>(ResultError error)`: Creates a failure result with error details.
- `Failure<TValue>(string code, string reason, string message)`: Shorthand to create a failure result with error
  details.

### `ResultError`

Represents an error with the following properties:

- `Code`: A string representing the error code.
- `Reason`: A brief reason for the error.
- `Message`: A detailed message explaining the error.

## License

This project is licensed under the MIT License.
