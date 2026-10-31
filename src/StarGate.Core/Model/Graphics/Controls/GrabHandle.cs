namespace StarGate.Core;

/// <summary>
/// Represents various types of handles that can be used for graphical manipulation.
/// </summary>
public enum GrabHandle
{
    None,
    TopLeft,
    TopCenter,
    TopRight,
    CenterLeft,
    CenterRight,
    BottomLeft,
    BottomCenter,
    BottomRight,
    Rotation,
    Move,
    Origin,
    StartAngle,
    SweepAngle,
    Anchor
}