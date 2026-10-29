namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Represents a fill style that uses a texture image. This class enables you to specify an image
/// as a fill, along with a wrapping mode that defines how the texture should be applied to
/// surfaces.
/// </summary>
public class TextureFill : FillStyle
{
    /// <summary>
    /// Gets the byte array representation of the image used as the texture fill. The image defines
    /// the visual content of the texture applied to surfaces.
    /// </summary>
    public byte[] Image { get; init; } = [];

    /// <summary>
    /// Gets the wrapping mode that specifies how the texture image is applied to a surface.
    /// The wrapping mode determines whether the texture is stretched or tiled when the texture
    /// size does not match the dimensions of the surface.
    /// </summary>
    public WrapMode Wrap { get; init; }
}