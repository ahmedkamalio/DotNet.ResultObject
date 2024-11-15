# Changelog

## v1.1.0

1. Error Categorization:
    - Added new `ErrorCategory` enum for standardized error classification
    - Introduced generic `ResultError<TErrorCategory>` for custom error categories
    - Added error category support to all Result types
    - Implemented error sanitization with configurable levels

2. Framework Support:
    - Added support for .NET 9.0
    - Multi-targeting both .NET 8.0 and .NET 9.0
    - Updated build process to handle multiple framework targets

3. Result Type Improvements:
    - Added `Unit` type for void-equivalent results
    - Introduced `Result.Success()` for parameterless success results
    - Enhanced generic constraints for non-nullable types
    - Improved type safety with stronger generic constraints

4. Error Handling Enhancements:
    - Added stack trace support via `WithStackTrace()`
    - Implemented error sanitization with `None`, `MessageOnly`, and `Full` levels
    - Added support for inner errors
    - Enhanced error message formatting

5. Build and Packaging:
    - Improved deterministic builds
    - Enhanced symbol package generation
    - Updated CI/CD pipeline for multi-framework builds
    - Added package validation checks

## v1.0.4

1. `Cast<T>()`:
    - Update the return type of `Cast<T>()` from `IResult<T>` to `Result<T>`.

## v1.0.3

1. `IResult`:
    - Introduced type-safe generics to `IResult`.
2. `Result` Class Changes:
    - Removed the old `ValueOrDefault` feature in favor of nullable `Value` and `Error`.
    - Removed the enforced exception for accessing values or errors incorrectly.
    - Added a new `Cast<T>()` method for safe type casting with error preservation.
    - Deprecated `ToFailureResult<T>()` in favor of the new `Cast<T>()` method.
3. Tests Updated:
    - Improved error handling validation in tests.
    - Removed exceptions where not necessary (like accessing `Value` on failure).
    - Updated tests to reflect the new `Cast<T>()` behavior.
