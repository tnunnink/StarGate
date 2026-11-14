using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StarGate.Core;

/// <summary>
/// Provides extension methods for object validation using data annotations.
/// </summary>
public static class ValidationExtension
{
    /// <summary>
    /// Validates the specified object instance by applying data annotations and returns the validation result.
    /// </summary>
    /// <param name="instance">The object instance to validate. Cannot be null.</param>
    /// <returns>
    /// A <see cref="GalaxyResult"/> indicating the validation outcome.
    /// If validation is successful, it returns an "Ok" result.
    /// Otherwise, it returns a failure result containing validation errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="instance"/> parameter is null.</exception>
    public static GalaxyResult Validate(this object instance)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        var context = new ValidationContext(instance);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(instance, context, results, validateAllProperties: true);

        if (results.Count == 0)
            return GalaxyResult.Ok();

        var errors = results.Select(r => new GalaxyError(ErrorCode.ValidationFailed, r.ErrorMessage)).ToArray();
        return GalaxyResult.Fail(errors);
    }

    /// <summary>
    /// Validates a collection of object instances by applying data annotations to each instance
    /// and aggregates the validation results into a single outcome.
    /// </summary>
    /// <param name="instances">
    /// The collection of object instances to validate. Cannot be null.
    /// </param>
    /// <returns>
    /// A <see cref="GalaxyResult"/> that represents the aggregated validation outcomes.
    /// If all instances are successfully validated, it returns an "Ok" result.
    /// Otherwise, it returns a failure result containing validation errors from all invalid instances.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="instances"/> parameter is null.</exception>
    public static GalaxyResult Validate(this IEnumerable<object> instances)
    {
        return GalaxyResult.Ok().ThenAll(instances, x => x.Validate());
    }
}