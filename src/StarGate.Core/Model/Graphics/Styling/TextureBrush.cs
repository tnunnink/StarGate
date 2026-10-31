namespace StarGate.Core;

/// <summary>
/// Represents a brush that uses a texture image to render visual elements.
/// A TextureBrush defines its appearance through an image source and a wrapping mode
/// that dictates how the image is applied to the target surface.
/// </summary>
public record TextureBrush : GraphicBrush
{
    /// <summary>
    /// The file path or URI of the texture image used by the brush to render visual elements.
    /// </summary>
    public required string Image { get; init; }

    /// <summary>
    /// Specifies the wrapping mode for the texture in the brush.
    /// Determines how the texture is applied when the dimensions of
    /// the texture do not match the target surface.
    /// </summary>
    public WrapMode Wrap { get; init; }
}