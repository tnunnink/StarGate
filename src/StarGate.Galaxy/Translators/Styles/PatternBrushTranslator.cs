using System.Xml.Linq;
using StarGate.Core;

namespace StarGate.Galaxy;

public class PatternBrushTranslator : GraphicTranslator<PatternBrush>
{
    /// <inheritdoc />
    public override PatternBrush? Read(XElement? target)
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
    public override XElement? Write(PatternBrush? source)
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