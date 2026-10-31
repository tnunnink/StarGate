namespace StarGate.Core;

/// <summary>
/// Defines a contract for models that support partial updates by merging
/// the current state with incoming changes.
/// </summary>
/// <typeparam name="T">The type of the model being merged.</typeparam>
public interface IGalaxyMergeable<T> where T : IGalaxyMergeable<T>
{
    /// <summary>
    /// Merges this instance (incoming desired state) with an existing instance (current state).
    /// Properties specified on this instance take precedence over the existing instance.
    /// </summary>
    /// <param name="current">The current state of the object, if present.</param>
    /// <returns>A new instance representing the combined, effective state.</returns>
    T Merge(T? current);
}