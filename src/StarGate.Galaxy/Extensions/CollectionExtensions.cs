using System.Collections.Generic;
using ArchestrA.GRAccess;

namespace StarGate.Galaxy.Extensions;

/// <summary>
/// Provides extension methods for working with enumerable sequences of specific types
/// in the ArchestrA ecosystem.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Converts an <see cref="IgObjects" /> collection into an <see cref="IEnumerable{T}" /> sequence that can be
    /// enumerated.
    /// </summary>
    /// <param name="collection">The <see cref="IgObjects" /> collection to convert.</param>
    /// <returns>An <see cref="IEnumerable{IgObject}" /> containing the elements of the specified collection.</returns>
    public static IEnumerable<IgObject> AsEnumerable(this IgObjects collection)
    {
        foreach (IgObject item in collection)
            yield return item;
    }

    /// <summary>
    /// Converts an <see cref="IAttributes" /> collection into an <see cref="IEnumerable{IAttribute}" /> sequence that can
    /// be enumerated.
    /// </summary>
    /// <param name="collection">The <see cref="IAttributes" /> collection to convert.</param>
    /// <returns>An <see cref="IEnumerable{IAttribute}" /> containing the elements of the specified collection.</returns>
    public static IEnumerable<IAttribute> AsEnumerable(this IAttributes collection)
    {
        foreach (IAttribute item in collection)
            yield return item;
    }
}