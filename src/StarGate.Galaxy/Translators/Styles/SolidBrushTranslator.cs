using System.Xml.Linq;
using StarGate.Core;
using StarGate.Galaxy;

namespace StarGate.Galaxy;

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

        var element = new XElement("SolidFill");
        element.AddColor("FillColor", source.Color);
        element.SetElementValue("Transparency", source.Transparency);
        return element;
    }
}