using System.ComponentModel;
using StarGate.Core.Enums;

namespace StarGate.Core;

/// <summary>
/// Represents a custom property defined on an ArchestrA graphic symbol.
/// </summary>
public record GraphicProperty
{
    /// <summary>
    /// Gets the unique name of the property.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the data type of the property value.
    /// </summary>
    public PropertyDataType DataType { get; init; } = PropertyDataType.Boolean;

    /// <summary>
    /// Gets the expression that defines the property's assigned value.
    /// </summary>
    public GraphicExpression Value { get; init; } = "false";

    /// <summary>
    /// Gets a value indicating whether the property is visible in design-time property browsers.
    /// </summary>
    [DefaultValue(true)]
    public bool Visible { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the property is locked against modification.
    /// </summary>
    public bool Locked { get; init; }

    /// <summary>
    /// Gets a value indicating whether the property value overrides an inherited definition.
    /// </summary>
    public bool Overridden { get; init; }

    /// <summary>
    /// Gets the optional description or documentation text for the property.
    /// </summary>
    public string? Description { get; init; }
}