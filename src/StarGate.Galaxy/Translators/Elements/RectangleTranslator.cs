using System.Xml.Linq;
using StarGate.Core;

namespace StarGate.Galaxy;

public class RectangleTranslator : GraphicTranslator<RectangleElement>
{
    private static readonly FillTranslator FillTranslator = new();
    private static readonly StrokeTranslator StrokeTranslator = new();

    /// <inheritdoc />
    public override RectangleElement? Read(XElement? target)
    {
        if (target is null) return null;

        return new RectangleElement
        {
            Name = target.GetRequiredText("@Name"),
            Bounds = target.GetBounds(),
            Origin = target.GetOrigin(),
            Angle = target.GetStruct("@Angle", int.Parse),
            Radius = target.GetStruct("@Radius", int.Parse),
            Flip = target.GetFlip(),
            Enabled = target.GetStruct("@Enabled", bool.Parse),
            Visible = target.GetStruct("@Visible", bool.Parse),
            Locked = target.GetStruct("@Locked", bool.Parse),
            TabStop = target.GetStruct("@TabStop", bool.Parse),
            TabOrder = target.GetStruct("@TabOrder", int.Parse),
            Faceplate = target.GetStruct("@FaceplateMode", bool.Parse),
            Anchor = target.GetEnum<AnchorType>("@Anchor"),
            Dock = target.GetEnum<DockType>("@Dock"),
            Fill = FillTranslator.Read(target.GetChild("FillStyleData")),
            Stroke = StrokeTranslator.Read(target.GetChild("LineStyleData"))
        };
    }

    /// <inheritdoc />
    public override XElement? Write(RectangleElement? source)
    {
        if (source is null) return null;

        var element = new XElement("Rectangle")
            .SetValue("@Name", source.Name)
            .SetBounds(source.Bounds)
            .SetOrigin(source.Origin)
            .SetValue("@Angle", source.Angle)
            .SetValue("@Radius", source.Radius)
            .SetValue("@Enabled", source.Enabled)
            .SetValue("@Visible", source.Visible)
            .SetValue("@Locked", source.Locked)
            .SetValue("@TabStop", source.TabStop)
            .SetValue("@TabOrder", source.TabOrder)
            .SetValue("@Faceplate", source.Faceplate)
            .SetValue("@Anchor", source.Anchor)
            .SetValue("@Dock", source.Dock)
            .SetValue("@HasFillStyleData", source.Fill is not null)
            .SetValue("@HasLineStyleData", source.Stroke is not null)
            .SetValue("@HasTextStyleData", source.Font is not null)
            .AddChild(FillTranslator.Write(source.Fill))
            .AddChild(StrokeTranslator.Write(source.Stroke));

        return element;
    }
}