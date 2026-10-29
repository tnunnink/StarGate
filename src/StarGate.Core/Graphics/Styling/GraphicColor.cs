using FormatException = System.FormatException;
using StringSplitOptions = System.StringSplitOptions;

namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Represents an RGBA color value used for styling graphic elements and symbols.
/// </summary>
/// <param name="R">The red component value (0–255).</param>
/// <param name="G">The green component value (0–255).</param>
/// <param name="B">The blue component value (0–255).</param>
/// <param name="A">The alpha (opacity) component value (0–255). Defaults to 255 (fully opaque).</param>
public readonly record struct GraphicColor(byte R, byte G, byte B, byte A = 255)
{
    /// <summary>
    /// Gets a predefined transparent color (RGBA: 0, 0, 0, 0).
    /// </summary>
    public static GraphicColor Transparent => new(0, 0, 0, 0);

    /// <summary>
    /// Gets a predefined opaque black color (RGBA: 0, 0, 0, 255).
    /// </summary>
    public static GraphicColor Black => new(0, 0, 0);

    /// <summary>
    /// Gets a predefined opaque white color (RGBA: 255, 255, 255, 255).
    /// </summary>
    public static GraphicColor White => new(255, 255, 255);

    /// <summary>
    /// Parses a color string in hexadecimal format (e.g., "#RRGGBB", "#RRGGBBAA", "RRGGBB", "RRGGBBAA") or
    /// space-delimited ArchestrA format ("R G B" or "R G B A").
    /// </summary>
    /// <param name="value">The string representation of the color to parse.</param>
    /// <returns>The parsed <see cref="GraphicColor"/> instance.</returns>
    /// <exception cref="FormatException">Thrown when <paramref name="value"/> is not in a recognized color format.</exception>
    public static GraphicColor Parse(string value)
    {
        if (TryParse(value, out var color))
            return color;

        throw new FormatException($"Invalid color format: '{value}'. Expected hex (#RRGGBB, #RRGGBBAA) or 'R G B'.");
    }

    /// <summary>
    /// Attempts to parse a color string in hexadecimal format or space-delimited ArchestrA format into a <see cref="GraphicColor"/>.
    /// </summary>
    /// <param name="value">The string representation of the color to parse.</param>
    /// <param name="color">When this method returns, contains the parsed <see cref="GraphicColor"/> if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
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

            var a = parts.Length > 3 && byte.TryParse(parts[3], out var parsedA) ? parsedA : (byte)255;
            color = new GraphicColor(r, g, b, a);
            return true;
        }

        // 2. Hex notation: "#RRGGBB" or "RRGGBB"
        var hex = value.StartsWith("#") ? value[1..] : value;

        switch (hex.Length)
        {
            case 6 when uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var rgb):
                color = new GraphicColor((byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
                return true;
            case 8 when uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var rgba):
                color = new GraphicColor((byte)(rgba >> 24), (byte)(rgba >> 16), (byte)(rgba >> 8), (byte)rgba);
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Formats the color as a hexadecimal string.
    /// </summary>
    /// <param name="includeAlpha">Determines whether to include the alpha channel in the output (#RRGGBBAA when <see langword="true"/>; #RRGGBB when <see langword="false"/>).</param>
    /// <returns>The hexadecimal string representation prefixed with '#'.</returns>
    public string ToHex(bool includeAlpha = false)
    {
        return includeAlpha
            ? $"#{R:X2}{G:X2}{B:X2}{A:X2}"
            : $"#{R:X2}{G:X2}{B:X2}";
    }

    /// <summary>
    /// Formats the color as a space-delimited RGB string ("R G B").
    /// </summary>
    /// <returns>A space-separated string containing the red, green, and blue components.</returns>
    // ReSharper disable once InconsistentNaming
    public string ToRGB() => $"{R} {G} {B}";

    /// <summary>
    /// Formats the color as a space-delimited RGBA string ("R G B A").
    /// </summary>
    /// <returns>A space-separated string containing the red, green, blue, and alpha components.</returns>
    // ReSharper disable once InconsistentNaming
    public string ToRGBA() => $"{R} {G} {B} {A}";

    /// <summary>
    /// Returns a hexadecimal string representation of the color, including alpha only when not fully opaque.
    /// </summary>
    /// <returns>A hexadecimal string representation of the color.</returns>
    public override string ToString() => ToHex(A < 255);

    /// <summary>
    /// Implicitly converts a color string into a <see cref="GraphicColor"/> instance via <see cref="Parse"/>.
    /// </summary>
    /// <param name="hex">The color string to parse.</param>
    /// <returns>The parsed <see cref="GraphicColor"/>.</returns>
    public static implicit operator GraphicColor(string hex) => Parse(hex);
}