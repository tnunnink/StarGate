using System.ComponentModel.DataAnnotations;

namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Represents the base class for defining fill styles of graphic symbols,
/// providing common properties and behaviors for derived fill style types.
/// </summary>
public abstract class FillStyle
{
    /// <summary>
    /// Transparency percent (0 = fully opaque, 100 = fully transparent).
    /// </summary>
    [Range(0.0, 100.0, ErrorMessage = "Transparency must be between 0 and 100.")]
    public double? Transparency { get; init; }
}