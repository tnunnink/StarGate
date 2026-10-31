namespace StarGate.Core;

/// <summary>
/// Represents the direction in which a fill operation can be applied.
/// This enumeration is typically used to specify the orientation of graphical
/// or layout-related fill behavior.
/// </summary>
public enum FillDirection
{
    /// <summary>
    /// Indicates a fill operation oriented upwards.
    /// Commonly used in graphical or layout systems to define that the fill behavior
    /// should progress in the direction from bottom to top.
    /// </summary>
    Up,

    /// <summary>
    /// Indicates a fill operation oriented downwards.
    /// Typically used in graphical or layout systems to specify that the fill behavior
    /// progresses from top to bottom.
    /// </summary>
    Down,

    /// <summary>
    /// Indicates a fill operation oriented to the left.
    /// Typically used in graphical or layout systems to define that the fill behavior
    /// should progress in the direction from right to left.
    /// </summary>
    Left,

    /// <summary>
    /// Indicates a fill operation oriented to the right.
    /// Commonly employed in graphical or layout systems to define that the fill behavior
    /// should progress in the direction from left to right.
    /// </summary>
    Right,

    /// <summary>
    /// Indicates a fill operation oriented diagonally upwards and to the right.
    /// Commonly used in graphical or layout systems to define that the fill behavior
    /// should progress in a direction combining upward and rightward movement.
    /// </summary>
    UpRight,

    /// <summary>
    /// Indicates a fill operation oriented diagonally upwards to the left.
    /// Commonly utilized in graphical or layout systems to define a fill behavior
    /// that progresses from the bottom-right corner toward the top-left corner.
    /// </summary>
    UpLeft,

    /// <summary>
    /// Indicates a fill operation oriented downwards and towards the right.
    /// Typically used in graphical or layout systems to define that the fill behavior
    /// progresses diagonally in a downward-right direction.
    /// </summary>
    DownRight,

    /// <summary>
    /// Indicates a fill operation oriented diagonally downwards and to the left.
    /// Commonly used in graphical or layout systems to define that the fill behavior
    /// should progress in a direction combining both downward and leftward movements.
    /// </summary>
    DownLeft
}