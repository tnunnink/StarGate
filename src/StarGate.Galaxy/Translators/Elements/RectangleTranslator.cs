using System;
using System.Linq;
using System.Xml.Linq;
using StarGate.Core;
using StarGate.Galaxy.Extensions;
using StarGate.Galaxy.Translators.Styles;

namespace StarGate.Galaxy.Translators.Elements;

/// <summary>
/// 
/// </summary>
public class RectangleTranslator : IGraphicTranslator<RectangleElement>
{
    private static readonly FillStyleTranslator FillStyleTranslator = new();

    /// <inheritdoc />
    public RectangleElement? Read(XElement? target)
    {
        if (target is null) return null;

        return new RectangleElement
        {
            Name = target.GetRequiredValue("@Name", s => s),
            Bounds = target.GetBounds(),
            Origin = target.GetOrigin(),
            Angle = target.GetStruct("@Angle", double.Parse),
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
            Fill = FillStyleTranslator.Read(target.GetChild("FillStyleData")),
        };
    }

    /// <inheritdoc />
    public XElement? Write(RectangleElement? source)
    {
        throw new NotImplementedException();
    }
}