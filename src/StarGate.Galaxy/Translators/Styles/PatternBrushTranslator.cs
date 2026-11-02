using System.Xml.Linq;
using StarGate.Core;
using StarGate.Galaxy;

namespace StarGate.Galaxy;

public class PatternBrushTranslator : IGraphicTranslator<PatternBrush>
{
    /// <inheritdoc />
    public PatternBrush? Read(XElement? target)
    {
        if (target is null) return null;

        return new PatternBrush
        {
            ForeColor = target.GetColor("ForeColor"),
            BackColor = target.GetColor("BackColor"),
            Hatch = target.GetEnum<HatchStyle>("HatchStyle"),
            Transparency = target.GetStruct("Transparency", double.Parse)
        };
    }

    /// <inheritdoc />
    public XElement? Write(PatternBrush? source)
    {
        if (source is null) return null;

        var element = new XElement("PatternFill");
        element.AddColor("ForeColor", source.ForeColor);
        element.AddColor("BackColor", source.BackColor);
        element.SetElementValue("Transparency", source.Transparency);
        element.SetElementValue("HatchStyle", source.Hatch);
        return element;
    }
}