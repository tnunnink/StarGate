using System;
using System.Xml.Linq;
using StarGate.Core;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Translators.Styles;

public class BrushTranslator : IGraphicTranslator<GraphicBrush>
{
    private static readonly SolidBrushTranslator SolidBrushTranslator = new();

    public GraphicBrush? Read(XElement? target)
    {
        if (target is null) return null;
        var type = target.GetRequiredValue("@FillType", s => s).ToLower();

        return type switch
        {
            "solid" => SolidBrushTranslator.Read(target.GetChild("SolidFill")),
            // todo remaining brushes
            // ReSharper disable once StringLiteralTypo explicit lower case.
            "nofill" => null,
            _ => throw new NotSupportedException("")
        };
    }

    public XElement? Write(GraphicBrush? source)
    {
        throw new NotImplementedException();
    }
}