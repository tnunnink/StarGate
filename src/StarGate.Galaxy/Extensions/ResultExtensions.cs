using System.Linq;
using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy.Extensions;

/// <summary>
/// Provides extension methods for converting GRAccess command results into <see cref="GalaxyResult"/>
/// and <see cref="GalaxyResult{TValue}"/> instances.
/// </summary>
public static class ResultExtensions
{
    /// <param name="result">The command result to convert.</param>
    extension(ICommandResult? result)
    {
        /// <summary>
        /// Converts an <see cref="ICommandResult"/> into a <see cref="GalaxyResult"/>.
        /// </summary>
        /// <returns>A <see cref="GalaxyResult"/> representing the success or failure of the command operation.</returns>
        public GalaxyResult ToResult()
        {
            if (result is null || result.Successful)
                return GalaxyResult.Ok();

            var error = new GalaxyError(result.ID.ToCoreType(), result.Text);
            return GalaxyResult.Fail([error]);
        }

        /// <summary>
        /// Converts an <see cref="ICommandResult"/> into a <see cref="GalaxyResult{TValue}"/> containing the specified value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to return with the result.</typeparam>
        /// <param name="value">The value to attach to the result.</param>
        /// <returns>A <see cref="GalaxyResult{TValue}"/> containing the value and representing the success or failure of the command operation.</returns>
        public GalaxyResult<TValue> ToResult<TValue>(TValue value)
        {
            if (result is null || result.Successful)
                return GalaxyResult.Ok(value);

            var error = new GalaxyError(result.ID.ToCoreType(), result.Text);
            return GalaxyResult.Fail(value, [error]);
        }
    }

    /// <param name="results">The collection of command results to convert.</param>
    extension(ICommandResults? results)
    {
        /// <summary>
        /// Converts an <see cref="ICommandResults"/> collection into a <see cref="GalaxyResult"/>.
        /// </summary>
        /// <returns>A <see cref="GalaxyResult"/> representing success if all commands succeeded, or containing errors for any failed commands.</returns>
        public GalaxyResult ToResult()
        {
            if (results is null || results.CompletelySuccessful)
                return GalaxyResult.Ok();

            var errors = results.Cast<ICommandResult>()
                .Where(result => !result.Successful)
                .Select(result => new GalaxyError(result.ID.ToCoreType(), result.Text))
                .ToArray();

            return GalaxyResult.Fail(errors);
        }

        /// <summary>
        /// Converts an <see cref="ICommandResults"/> collection into a <see cref="GalaxyResult{TValue}"/> containing the specified value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to return with the result.</typeparam>
        /// <param name="value">The value to attach to the result.</param>
        /// <returns>A <see cref="GalaxyResult{TValue}"/> containing the value and representing success if all commands succeeded, or containing errors for any failed commands.</returns>
        public GalaxyResult<TValue> ToResult<TValue>(TValue value)
        {
            if (results is null || results.CompletelySuccessful)
                return GalaxyResult.Ok(value);

            var errors = results.Cast<ICommandResult>()
                .Where(result => !result.Successful)
                .Select(result => new GalaxyError(result.ID.ToCoreType(), result.Text))
                .ToArray();

            return GalaxyResult.Fail(value, errors);
        }
    }
}