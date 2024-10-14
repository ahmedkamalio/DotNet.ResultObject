# Changelog

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
