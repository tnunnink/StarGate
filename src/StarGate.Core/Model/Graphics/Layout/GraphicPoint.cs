namespace StarGate.Core;

/// <summary>
/// Represents a point in a 2D coordinate system defined by X and Y coordinates.
/// </summary>
public readonly record struct GraphicPoint()
{
    /// <summary>
    /// Creates a new <see cref="GraphicPoint" /> given the X and Y coordinates.
    /// </summary>
    /// <param name="x">The X coordinate that represents the location of the point.</param>
    /// <param name="y">The Y coordinate that represents the location of the point.</param>
    public GraphicPoint(double x, double y) : this()
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets the horizontal X-coordinate of the point.
    /// </summary>
    public double X { get; init; }

    /// <summary>
    /// Gets the vertical Y-coordinate of the point.
    /// </summary>
    public double Y { get; init; }
}