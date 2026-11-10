namespace StarGate.Core;

/// <summary>
/// Defines the available brush types used to paint or fill graphical content.
/// </summary>
public enum BrushKind
{
    /// <summary>
    /// Represents a brush that paints or fills graphical content with a single uniform color.
    /// </summary>
    Solid,

    /// <summary>
    /// Represents a brush that paints or fills graphical content using a gradual transition between colors.
    /// </summary>
    Gradient,

    /// <summary>
    /// Represents a brush that paints or fills graphical content using a repeated hatch or pattern style.
    /// </summary>
    Pattern,

    /// <summary>
    /// Represents a brush that paints or fills graphical content using an image-based texture.
    /// </summary>
    Texture
}