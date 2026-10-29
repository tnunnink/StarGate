using StarGate.Core.Abstractions;

namespace StarGate.Core.Objects;

/// <summary>
/// Represents configuration settings for an object attribute, including its value, lock state, and security permissions.
/// </summary>
public class AttributeConfig : IMergeable<AttributeConfig>
{
    /// <summary>
    /// Gets or sets the configured value of the attribute.
    /// </summary>
    public object? Value { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the attribute is locked against modification.
    /// </summary>
    public bool? Locked { get; init; }

    /// <summary>
    /// Gets or sets the security access level required for the attribute.
    /// </summary>
    public Security? Security { get; init; }

    /// <inheritdoc />
    public AttributeConfig Merge(AttributeConfig? current)
    {
        if (current is null) return this;

        return new AttributeConfig
        {
            // Source wins if specified; otherwise retain current
            Value = Value ?? current.Value,
            Locked = Locked ?? current.Locked,
            Security = Security ?? current.Security
        };
    }
}