using System.Xml.Linq;
using StarGate.Core;

namespace StarGate.Galaxy;

public class StrokeTranslator : GraphicTranslator<StrokeStyle>
{
    private static readonly BrushTranslator BrushTranslator = new();

    /// <inheritdoc />
    public override StrokeStyle? Read(XElement? target)
    {
        if (target is null) return null;

        return new StrokeStyle
        {
            Pattern = target.GetEnum<StrokePattern>("LineStyle.@LinePattern"),
            Width = target.GetStruct("LineStyle.@Weight", int.Parse),
            Brush = BrushTranslator.Read(target.GetChild("LineStyle.FillStyle")),
            StartCap = target.GetOptionalEnum<StrokeCap>("@StartCap"),
            EndCap = target.GetOptionalEnum<StrokeCap>("@EndCap")
        };
    }

    /// <inheritdoc />
    public override XElement? Write(StrokeStyle? source)
    {
        if (source is null) return null;

        var element = new XElement("LineStyleData")
            .AddChild("LineStyle", e =>
            {
                e.SetValue("@Weight", source.Width);
                e.SetValue("@LinePattern", source.Pattern);
                e.AddFillStyle(source.Brush, b => BrushTranslator.Write(b));
            })
            .SetValue("@StartCap", source.StartCap)
            .SetValue("@EndCap", source.StartCap);

        return element;
    }
}