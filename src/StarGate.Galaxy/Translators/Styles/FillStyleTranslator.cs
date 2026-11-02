using System.Xml.Linq;
using StarGate.Core;
using StarGate.Galaxy;

namespace StarGate.Galaxy;

/// <summary>
/// Provides functionality to translate between an XML representation and a <see cref="FillStyle"/> domain model object.
/// </summary>
/// <remarks>
/// This translator handles the mapping of fill style properties such as orientation, direction,
/// and brush configuration from an XML representation to a <see cref="FillStyle"/> object and vice versa.
/// </remarks>
public class FillStyleTranslator : IGraphicTranslator<FillStyle>
{
    private static readonly BrushTranslator BrushTranslator = new();

    /// <inheritdoc />
    public FillStyle? Read(XElement? target)
    {
        if (target is null) return null;

        return new FillStyle
        {
            Brush = BrushTranslator.Read(target.GetChild("FillStyle")),
            Unfilled = BrushTranslator.Read(target.GetChild("UnfilledStyle")),
            Orientation = target.GetEnum<FillOrientation>("FillOrientation"),
            Direction = target.GetFillDirection(),
            Vertical = target.GetStruct("@VerticalPercentFill", double.Parse),
            Horizontal = target.GetStruct("@HorizontalPercentFill", double.Parse)
        };
    }

    /// <inheritdoc />
    public XElement? Write(FillStyle? source)
    {
        if (source is null) return null;

        var element = new XElement("FillStyleData")
            .AddFillStyle(source.Brush, b => BrushTranslator.Write(b))
            .AddUnfilledStyle(source.Unfilled, b => BrushTranslator.Write(b))
            .SetValue("FillOrientation", source.Orientation)
            .SetFillDirection(source.Direction)
            .SetValue("VerticalPercentFill", source.Vertical)
            .SetValue("VerticalHorizontalFill", source.Horizontal);

        return element;
    }
}