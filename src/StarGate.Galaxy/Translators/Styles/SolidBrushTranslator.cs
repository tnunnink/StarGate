using System.Xml.Linq;
using StarGate.Core;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Translators.Styles;

/// <summary>
/// Provides functionality for translating a <see cref="SolidBrush"/> object to and from its ArchestrA XML representation.
/// This class implements the <see cref="IGraphicTranslator{T}"/> interface for <see cref="SolidBrush"/>.
/// </summary>
public class SolidBrushTranslator : IGraphicTranslator<SolidBrush>
{
    /// <inheritdoc />
    public SolidBrush? Read(XElement? target)
    {
        if (target is null) return null;

        return new SolidBrush
        {
            Color = target.GetColor("FillColor"),
            Transparency = target.GetStruct("Transparency", double.Parse)
        };
    }

    /// <inheritdoc />
    public XElement? Write(SolidBrush? source)
    {
        if (source is null) return null;

        var element = new XElement("SolidColor");

        element.Add(new XElement("FillColor",
            new XAttribute("R", source.Color.R),
            new XAttribute("G", source.Color.G),
            new XAttribute("B", source.Color.B),
            new XAttribute("A", source.Color.A)
        ));

        element.Add(new XElement("Transparency", source.Transparency));

        return element;
    }
}