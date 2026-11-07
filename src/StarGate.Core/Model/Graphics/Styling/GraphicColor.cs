using System;
using System.Globalization;

namespace StarGate.Core;

/// <summary>
/// Represents an RGB color value used for styling graphic elements and symbols.
/// </summary>
/// <param name="R">The red component value (0–255).</param>
/// <param name="G">The green component value (0–255).</param>
/// <param name="B">The blue component value (0–255).</param>
public readonly record struct GraphicColor(byte R, byte G, byte B)
{
    /// <summary>
    /// Gets a predefined opaque black color (RGB: 0, 0, 0).
    /// </summary>
    public static GraphicColor Black => new(0, 0, 0);

    /// <summary>
    /// Gets a predefined opaque white color (RGB: 255, 255, 255).
    /// </summary>
    public static GraphicColor White => new(255, 255, 255);

    /// <summary>
    /// Parses a string representation of a color into a <see cref="GraphicColor" /> instance.
    /// </summary>
    /// <param name="value">The color string to parse (hex format "#RRGGBB"/"RRGGBB" or space-delimited "R G B").</param>
    /// <returns>The parsed <see cref="GraphicColor" />.</returns>
    /// <exception cref="FormatException">Thrown when <paramref name="value" /> is not in a recognized color format.</exception>
    public static GraphicColor Parse(string value)
    {
        if (TryParse(value, out var color))
            return color;

        throw new FormatException($"Invalid color format: '{value}'. Expected hex #RRGGBB or 'R G B'.");
    }

    /// <summary>
    /// Attempts to parse a string representation of a color into a <see cref="GraphicColor" /> instance.
    /// </summary>
    /// <param name="value">The color string to parse, or <see langword="null" />.</param>
    /// <param name="color">When this method returns, contains the parsed <see cref="GraphicColor" /> if the conversion succeeded, or the default value if failed.</param>
    /// <returns><see langword="true" /> if <paramref name="value" /> was successfully parsed; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(string? value, out GraphicColor color)
    {
        color = default;

        if (value is null || string.IsNullOrWhiteSpace(value))
            return false;

        value = value.Trim();

        // 1. ArchestrA space-delimited: "255 128 0"
        if (value.Contains(" "))
        {
            var parts = value.Split([' '], StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 3
                || !byte.TryParse(parts[0], out var r)
                || !byte.TryParse(parts[1], out var g)
                || !byte.TryParse(parts[2], out var b)) return false;

            color = new GraphicColor(r, g, b);
            return true;
        }

        // 2. Hex notation: "#RRGGBB" or "RRGGBB"
        var hex = value.StartsWith("#") ? value[1..] : value;

        switch (hex.Length)
        {
            case 6 when uint.TryParse(hex, NumberStyles.HexNumber, null, out var rgb):
                color = new GraphicColor((byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Formats the color as a hex color string (e.g., "#RRGGBB").
    /// </summary>
    /// <returns>A hex string representing the color.</returns>
    public string ToHex() => $"#{R:X2}{G:X2}{B:X2}";

    /// <summary>
    /// Formats the color as a space-delimited RGB string ("R G B").
    /// </summary>
    /// <returns>A space-separated string containing the red, green, and blue components.</returns>
    // ReSharper disable once InconsistentNaming
    public string ToRGB() => $"{R} {G} {B}";

    /// <summary>
    /// Returns the hex string representation of the color.
    /// </summary>
    /// <returns>A string representation formatted as hex "#RRGGBB".</returns>
    public override string ToString() => ToHex();

    /// <summary>
    /// Implicitly converts a color string into a <see cref="GraphicColor" /> instance via <see cref="Parse" />.
    /// </summary>
    /// <param name="hex">The color string to parse.</param>
    /// <returns>The parsed <see cref="GraphicColor" />.</returns>
    public static implicit operator GraphicColor(string hex) => Parse(hex);
}