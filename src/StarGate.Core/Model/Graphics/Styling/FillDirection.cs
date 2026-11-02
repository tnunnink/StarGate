using System;

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
    /// Commonly used in graphical or layout systems to define a fill behavior
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

/// <summary>
/// Provides extension methods for handling FillDirection operations.
/// </summary>
public static class FillDirectionExtension
{
    /// <summary>
    /// Combines a vertical and a horizontal FillDirection to calculate a diagonal FillDirection.
    /// </summary>
    /// <param name="vertical">The vertical FillDirection, which must be either 'Up' or 'Down'.</param>
    /// <param name="horizontal">The horizontal FillDirection, which must be either 'Left' or 'Right'.</param>
    /// <returns>A combined FillDirection representing the diagonal direction.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the vertical direction is not 'Up' or 'Down', or if the horizontal direction is not 'Left' or 'Right'.
    /// </exception>
    public static FillDirection Combine(this FillDirection vertical, FillDirection horizontal)
    {
        if (vertical is not FillDirection.Up and not FillDirection.Down)
            throw new ArgumentException("Vertical direction must have value of 'Up' or 'Down'");

        if (horizontal is not FillDirection.Left and not FillDirection.Right)
            throw new ArgumentException("Vertical direction must have value of 'Left' or 'Right'");

        return (vertical, horizontal) switch
        {
            (FillDirection.Up, FillDirection.Right) => FillDirection.UpRight,
            (FillDirection.Up, FillDirection.Left) => FillDirection.UpLeft,
            (FillDirection.Down, FillDirection.Right) => FillDirection.DownRight,
            (FillDirection.Down, FillDirection.Left) => FillDirection.DownLeft,
            _ => throw new ArgumentException("")
        };
    }
}