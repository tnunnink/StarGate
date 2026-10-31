using System;
using System.Xml.Linq;
using StarGate.Core;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Translators.Styles;

public class StrokeTranslator : IGraphicTranslator<StrokeStyle>
{
    public string LocalName => "LineStyleData";

    public StrokeStyle? Read(XElement? target)
    {
        if (target is null) return null;

        return new StrokeStyle
        {
            Pattern = target.GetEnum<StrokePattern>("LineStyle.@LinePattern"),
            Width = target.GetStruct("LineStyle.@Weight", int.Parse),
            /*Color =*/
            /*StartCap = target.GetValue("@StartCap", s => Enum.Parse(typeof(StrokeCap), s))*/
        };
    }

    public XElement? Write(StrokeStyle? source)
    {
        throw new System.NotImplementedException();
    }
}