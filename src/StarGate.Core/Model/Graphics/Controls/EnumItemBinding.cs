namespace StarGate.Core;

/// <summary>
/// An item source populated dynamically from an attribute's enumerated states.
/// </summary>
public class EnumItemBinding : ItemBinding
{
    public static EnumItemBinding Instance { get; } = new();
}