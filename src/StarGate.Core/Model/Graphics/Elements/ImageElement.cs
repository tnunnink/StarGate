namespace StarGate.Core;

/// <summary>
/// Represents an image graphic element supporting raster and vector images.
/// </summary>
public class ImageElement : GraphicElement
{
    /// <inheritdoc />
    public override ElementKind Kind => ElementKind.Image;

    /// <summary>
    /// Gets the path or asset reference to the image file.
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Gets how the image fills the element bounds.
    /// </summary>
    public ImageStyle FillStyle { get; set; } = ImageStyle.Normal;

    /// <summary>
    /// Gets the alignment of the image within its bounds.
    /// </summary>
    public GraphicAlignment Alignment { get; set; } = GraphicAlignment.Centers;
}