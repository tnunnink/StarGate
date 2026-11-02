using System.Xml.Linq;
using StarGate.Core;
using StarGate.Galaxy;

namespace StarGate.Galaxy;

public class BrushTranslator : IGraphicTranslator<GraphicBrush>
{
    private static readonly SolidBrushTranslator SolidTranslator = new();
    private static readonly PatternBrushTranslator PatternTranslator = new();

    /// <inheritdoc />
    public GraphicBrush? Read(XElement? target)
    {
        if (target is null) return null;
        var type = target.GetRequiredValue("@FillType", s => s.ToLower());

        return type switch
        {
            "solid" => SolidTranslator.Read(target.GetChild("SolidFill")),
            "pattern" => PatternTranslator.Read(target.GetChild("PatternFill")),
            _ => null
        };
    }

    /// <inheritdoc />
    public XElement? Write(GraphicBrush? source)
    {
        return source switch
        {
            SolidBrush solid => SolidTranslator.Write(solid),
            PatternBrush pattern => PatternTranslator.Write(pattern),
            _ => null   
        };
    }
}