using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace StarGate.Core;

/// <summary>
/// Represents an abstract base class for defining a graphical brush used to render visual elements.
/// A GraphicBrush typically includes properties such as transparency to define its behavior.
/// </summary>
public abstract record GraphicBrush
{
    /// <summary>
    /// Transparency percent (0 = fully opaque, 100 = fully transparent).
    /// </summary>
    [Range(0.0, 100.0, ErrorMessage = "Transparency must be between 0 and 100.")]
    [DefaultValue(0.0)]
    [YamlMember(Order = 10)]
    public double Transparency { get; init; }

    /// <summary>
    /// Defines an implicit conversion operator from a <see cref="GraphicColor"/> to a <see cref="GraphicBrush"/>.
    /// Enables the seamless creation of a <see cref="SolidBrush"/> instance when assigning a <see cref="GraphicColor"/>
    /// to a property or variable of type <see cref="GraphicBrush"/>.
    /// </summary>
    /// <param name="color">The <see cref="GraphicColor"/> to be converted into a <see cref="GraphicBrush"/>.</param>
    /// <returns>A new instance of <see cref="SolidBrush"/> with the specified <see cref="GraphicColor"/>.</returns>
    public static implicit operator GraphicBrush(GraphicColor color) => new SolidBrush { Color = color };

    /// <summary>
    /// Defines an implicit conversion operator from a hexadecimal color string to a <see cref="GraphicBrush"/>.
    /// This allows a string representing a color in hexadecimal format to be seamlessly converted
    /// into a <see cref="SolidBrush"/> instance with the specified color.
    /// </summary>
    /// <param name="hex">The hexadecimal string representing the color to be converted.</param>
    /// <returns>A new instance of <see cref="SolidBrush"/> with the color parsed from the specified hexadecimal string.</returns>
    public static implicit operator GraphicBrush(string hex) => new SolidBrush { Color = GraphicColor.Parse(hex) };
}