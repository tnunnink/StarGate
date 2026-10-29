namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Defines the shape used at the start or end of a line stroke in graphic rendering.
/// </summary>
/// <remarks>
/// StrokeCap specifies the style of line caps in graphics, determining how the ends
/// of a line are rendered. Various cap styles are available, enabling customization
/// in visual representation for strokes.
/// </remarks>
public enum StrokeCap
{
    Flat,
    Square,
    Round,
    Triangle,
    SquareAnchor,
    RoundAnchor,
    DiamondAnchor,
    ArrowAnchor
}