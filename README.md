# ResultObject

<img src="https://raw.githubusercontent.com/ahmedkamalio/DotNet.ResultObject/main/icon.png" alt="ResultObject Icon" width="100" height="100">

## Overview

The **ResultObject** package provides a utility to handle the outcome of operations, either as a success or failure,
without relying on exceptions. This promotes cleaner code by clearly separating success from failure scenarios and
encapsulating error information.

## Features

- **Success/Failure Handling**: Easily manage the outcome of operations with type-safe results.
- **Generic Result Type**: Results can encapsulate any type of value.
- **Error Handling**: Detailed error information available in case of failures.
- **Type-Safe Casting**: Use `Cast<T>()` to transform the result's value while maintaining error information.
- **Nullable Value Access**: Retrieve values or errors directly without throwing exceptions.

## Installation

You can install the **ResultObject** package via NuGet:

```bash
dotnet add package ResultObject
```

Alternatively, use the Visual Studio package manager UI to search for "ResultObject."

## Usage

### Basic Result Example

```csharp
var successResult = Result.Success(42);
var failureResult = Result.Failure<int>("404", "Not Found", "The requested resource was not found.");
```

### Checking Success or Failure

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

### Type Casting Results

Use `Cast<T>()` to cast the value to a different type while preserving error details on failure.

```csharp
var result = Result.Success<object>("Test Value");
var castResult = result.Cast<string>();

if (castResult.IsSuccess)
{
    Console.WriteLine("Successfully cast value: " + castResult.Value);
}
```

### Error Handling

Create and inspect detailed errors with `ResultError`.

```csharp
var error = new ResultError("500", "Server Error", "An unexpected error occurred.");
var failedResult = Result.Failure<int>(error);

Console.WriteLine($"Error Code: {failedResult.Error.Code}");
```

### Handling Nullable Values

A result with a `null` value is treated as a failure.

```csharp
var result = Result.Success<string?>(null);

if (result.IsFailure)
{
    Console.WriteLine("Result is not successful.");
}
```

## API Reference

### `IResult<TValue>`

- `IsSuccess`: Indicates if the operation succeeded.
- `IsFailure`: Indicates if the operation failed.
- `Value`: The value of the operation if successful, otherwise `null`.
- `Error`: The error information if the operation failed, otherwise `null`.

### `Result<TValue>`

- **Methods**:
    - `Cast<T>()`: Safely cast the result's value to a different type, or propagate the error if the cast fails.

### `Result`

- **Static Methods**:
    - `Success<TValue>(TValue value)`: Creates a success result.
    - `Failure<TValue>(ResultError error)`: Creates a failure result with error details.
    - `Failure<TValue>(string code, string reason, string message)`: Creates a failure result with a detailed error.

### `ResultError`

- **Properties**:
    - `Code`: A code representing the type of error.
    - `Reason`: A brief reason for the error.
    - `Message`: A detailed message describing the error.

## License

This project is licensed under the MIT License.
