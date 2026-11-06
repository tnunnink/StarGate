namespace StarGate.Core;

/// <summary>
/// Represents the rectangular bounding box of a graphic element,
/// defined by its position coordinates and dimensions.
/// </summary>
public readonly record struct GraphicBounds()
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="top"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public GraphicBounds(double left, double top, double width, double height) : this()
    {
        Left = left;
        Top = top;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets the horizontal coordinate of the left edge of the graphic box.
    /// </summary>
    public double Left { get; init; }

    /// <summary>
    /// Gets the vertical coordinate of the top edge of the graphic box.
    /// </summary>
    public double Top { get; init; }

    /// <summary>
    /// Gets the width of the graphic box.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets the height of the graphic box.
    /// </summary>
    public double Height { get; init; }
}