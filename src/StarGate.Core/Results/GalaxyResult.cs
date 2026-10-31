using System;
using System.Collections.Generic;
using System.Linq;

namespace StarGate.Core;

/// <summary>
/// Represents the outcome of an operation, offering facilities to track success, errors,
/// and associated messages. Provides mechanisms for error reporting and result merging.
/// </summary>
public class GalaxyResult
{
    private readonly List<GalaxyError> _errors;

    protected GalaxyResult(GalaxyError?[] errors)
    {
        _errors = errors.Where(e => e is not null).Cast<GalaxyError>().ToList();
    }

    /// <summary>
    /// Gets a value indicating whether the command or operation succeeded.
    /// </summary>
    public bool Success => _errors.Count == 0;

    /// <summary>
    /// Gets a descriptive message summarizing the result of the operation.
    /// </summary>
    /// <remarks>
    /// The message indicates whether the operation succeeded or failed.
    /// If the operation failed, it includes either the details of a single error or the total count of errors encountered.
    /// </remarks>
    public string Message => GetMessage();

    /// <summary>
    /// Gets the collection of errors associated with the operation.
    /// </summary>
    public IEnumerable<GalaxyError> Errors => _errors;

    /// <summary>
    /// Merges the current <see cref="GalaxyResult" /> instance with another <see cref="GalaxyResult" /> instance,
    /// combining their errors into a single result.
    /// </summary>
    /// <param name="other">The other <see cref="GalaxyResult" /> instance to merge with the current instance.</param>
    /// <returns>A new <see cref="GalaxyResult" /> containing the combined errors from both instances.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="other" /> parameter is null.</exception>
    public GalaxyResult Merge(GalaxyResult other)
    {
        if (other is null)
            throw new ArgumentNullException(nameof(other));

        return new GalaxyResult(_errors.Concat(other._errors).ToArray());
    }

    /// <summary>
    /// Executes the next function only if the current result succeeded.
    /// If the current result already failed, execution skips and the existing failure is returned.
    /// If the next function executes, its result is returned (or merged).
    /// </summary>
    public GalaxyResult Then(Func<GalaxyResult> next)
    {
        if (!Success) return this;
        var result = next();
        return Merge(result);
    }

    /// <summary>
    /// Executes a specified action for each item in the given collection and aggregates
    /// the results into a single <see cref="GalaxyResult" />. The process halts if the current
    /// result indicates failure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the provided collection.</typeparam>
    /// <param name="items">A collection of items to process.</param>
    /// <param name="action">A function that processes each item and returns a <see cref="GalaxyResult" />.</param>
    /// <returns>
    /// A <see cref="GalaxyResult" /> that aggregates the outcomes of the executed actions. If any
    /// action results in a failure, the aggregated result will indicate failure.
    /// </returns>
    public GalaxyResult ThenAll<T>(IEnumerable<T> items, Func<T, GalaxyResult> action)
    {
        if (!Success) return this;
        return items.Aggregate(this, (result, item) => result.And(() => action(item)));
    }

    /// <summary>
    /// Chains the current <see cref="GalaxyResult" /> with another operation, merging its result
    /// with the current instance's errors.
    /// </summary>
    /// <param name="step">
    /// A function that returns a new <see cref="GalaxyResult" /> to be merged with the current instance.
    /// </param>
    /// <returns>
    /// A new <see cref="GalaxyResult" /> instance that combines the errors from the current instance
    /// with the errors from the result of the provided <paramref name="step" /> function.
    /// </returns>
    public GalaxyResult And(Func<GalaxyResult> step)
    {
        var result = step();
        return Merge(result);
    }

    /// <summary>
    /// Creates a new <see cref="GalaxyResult" /> instance representing a successful operation with no errors.
    /// </summary>
    /// <returns>A <see cref="GalaxyResult" /> instance indicating success.</returns>
    public static GalaxyResult Ok()
    {
        return new GalaxyResult([]);
    }

    /// <summary>
    /// Creates a new <see cref="GalaxyResult" /> instance containing the specified errors.
    /// Null values in the input are excluded from the resulting instance.
    /// </summary>
    /// <param name="errors">
    /// A collection of <see cref="GalaxyError" /> objects to include in the result. Null values are
    /// ignored.
    /// </param>
    /// <returns>A <see cref="GalaxyResult" /> instance containing the valid, non-null errors provided in the input.</returns>
    public static GalaxyResult Fail(GalaxyError[] errors)
    {
        return new GalaxyResult(errors);
    }

    /// <summary>
    /// Creates a failed <see cref="GalaxyResult" /> instance with the specified error code and message.
    /// </summary>
    /// <param name="code">The error code indicating the reason for the failure.</param>
    /// <param name="message">The error message providing details about the failure.</param>
    /// <returns>A new <see cref="GalaxyResult" /> containing the specified error.</returns>
    public static GalaxyResult Fail(ErrorCode code, string message)
    {
        return new GalaxyResult([new GalaxyError(code, message)]);
    }

    /// <summary>
    /// Creates a new <see cref="GalaxyResult" /> instance that represents a successful result with no errors.
    /// </summary>
    /// <returns>A <see cref="GalaxyResult" /> instance representing success.</returns>
    public static GalaxyResult<T> Ok<T>(T value)
    {
        return new GalaxyResult<T>(value);
    }

    /// <summary>
    /// Creates a new <see cref="GalaxyResult{T}" /> instance containing the specified value and errors.
    /// Null values in the error input are excluded from the resulting instance.
    /// </summary>
    /// <typeparam name="T">The type of value produced by the operation.</typeparam>
    /// <param name="value">The underlying value associated with the result.</param>
    /// <param name="errors">
    /// A collection of <see cref="GalaxyError" /> objects to include in the result. Null values are
    /// ignored.
    /// </param>
    /// <returns>A <see cref="GalaxyResult{T}" /> instance containing the provided value and valid, non-null errors.</returns>
    public static GalaxyResult<T> Fail<T>(T value, GalaxyError[] errors)
    {
        return new GalaxyResult<T>(value, errors);
    }

    /// <summary>
    /// Creates a failure result for the specified value with the provided error code and message.
    /// </summary>
    /// <typeparam name="T">The type of the value associated with the failure result.</typeparam>
    /// <param name="value">The value associated with the failure result.</param>
    /// <param name="code">The error code representing the reason for the failure.</param>
    /// <param name="message">The error message describing the failure.</param>
    /// <returns>A <see cref="GalaxyResult{T}" /> containing the specified value and associated error information.</returns>
    public static GalaxyResult<T> Fail<T>(T value, ErrorCode code, string message)
    {
        return new GalaxyResult<T>(value, new GalaxyError(code, message));
    }

    /// <summary>
    /// Constructs a descriptive message summarizing the result of the operation based on the presence and quantity of
    /// errors.
    /// </summary>
    /// <returns>
    /// A string that indicates whether the operation succeeded or failed.
    /// If the operation failed, the message includes either a single error detail or the total count of errors
    /// encountered.
    /// </returns>
    private string GetMessage()
    {
        return _errors.Count switch
        {
            0 => "Operation completed successfully",
            1 => $"Operation failed with error: {_errors[0]}",
            _ => $"Operation encountered {_errors.Count} errors during execution. Check errors for details."
        };
    }
}

/// <summary>
/// Represents the result of an operation, containing either a successful value or a collection of errors.
/// </summary>
/// <typeparam name="T">The type of value produced by the operation.</typeparam>
/// <param name="value">The underlying value of the operation.</param>
/// <param name="errors">An optional collection of errors encountered during the operation.</param>
public class GalaxyResult<T>(T value, params GalaxyError?[] errors) : GalaxyResult(errors)
{
    /// <summary>
    /// Gets the underlying value of the operation, as produced by the result.
    /// </summary>
    public T Value { get; } = value;

    /// <summary>
    /// Merges this result with another result by combining their errors and merging their values using the specified
    /// function.
    /// </summary>
    /// <param name="other">The other result to merge with this instance.</param>
    /// <param name="merge">The function used to combine the two values.</param>
    /// <returns>A new <see cref="GalaxyResult{T}" /> containing the merged value and combined errors.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="other" /> or <paramref name="merge" /> is
    /// <see langword="null" />.
    /// </exception>
    public GalaxyResult<T> Merge(GalaxyResult<T> other, Func<T, T, T> merge)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));
        if (merge is null) throw new ArgumentNullException(nameof(merge));

        var mergedValue = merge.Invoke(Value, other.Value);
        var mergedErrors = Errors.Concat(other.Errors).ToArray();
        return new GalaxyResult<T>(mergedValue, mergedErrors);
    }

    /// <summary>
    /// Executes a function that produces the next <see cref="GalaxyResult{T}" /> if the current operation is successful
    /// and merges the resulting instance with the current instance.
    /// </summary>
    /// <param name="next">A function that takes the current value and returns the next <see cref="GalaxyResult{T}" />.</param>
    /// <returns>
    /// A new <see cref="GalaxyResult{T}" /> instance that represents the result of the current operation
    /// merged with the result of the subsequent operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="next" /> parameter is null.</exception>
    public GalaxyResult Then(Func<T, GalaxyResult<T>> next)
    {
        if (!Success) return this;
        var result = next(Value);
        return Merge(result);
    }

    /// <summary>
    /// Chains the current <see cref="GalaxyResult{T}" /> with a subsequent result-producing function,
    /// aggregating any errors that occur in the process.
    /// </summary>
    /// <param name="next">
    /// A function that takes the existing value of the current result and returns a new
    /// <see cref="GalaxyResult{T}" />.
    /// </param>
    /// <returns>A combined <see cref="GalaxyResult" /> that merges the results and errors from both operations.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="next" /> parameter is null.</exception>
    public GalaxyResult And(Func<T, GalaxyResult<T>> next)
    {
        var result = next(Value);
        return Merge(result);
    }
}