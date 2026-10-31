namespace StarGate.Core;

/// <summary>
/// An item source populated dynamically from a Galaxy array attribute.
/// </summary>
public class ArrayItemBinding(string arrayReference) : ItemBinding
{
    public string Reference { get; init; } = arrayReference;
}