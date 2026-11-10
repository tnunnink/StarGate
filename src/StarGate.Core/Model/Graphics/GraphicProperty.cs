using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using StarGate.Core.Enums;

namespace StarGate.Core;

/// <summary>
/// Represents a custom property defined on an ArchestrA graphic symbol.
/// </summary>
public class GraphicProperty
{
    /// <summary>
    /// Gets the unique name of the property.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the data type of the property value.
    /// </summary>
    public PropertyDataType DataType { get; set; } = PropertyDataType.Boolean;

    /// <summary>
    /// Gets the expression that defines the property's assigned value.
    /// </summary>
    public GraphicExpression Value { get; set; } = "false";

    /// <summary>
    /// Gets a value indicating whether the property is visible in design-time property browsers.
    /// </summary>
    [DefaultValue(true)]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether the property is locked against modification.
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>
    /// Gets a value indicating whether the property value overrides an inherited definition.
    /// </summary>
    public bool Overridden { get; set; }

    /// <summary>
    /// Gets the optional description or documentation text for the property.
    /// </summary>
    public string? Description { get; set; }
}