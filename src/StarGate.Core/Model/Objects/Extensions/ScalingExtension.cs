namespace StarGate.Core;

/// <summary>
/// Represents an extension for scaling raw and engineering unit values.
/// This class provides properties for configuring scaling parameters
/// such as raw maximum, raw minimum, engineering units maximum,
/// engineering units minimum, and conversion modes. It also supports
/// clamping of values within specified ranges.
/// </summary>
[GalaxyExtension("ScalingExtension", ExtensionType.Attribute)]
public class ScalingExtension : IGalaxyMergeable<ScalingExtension>, IAttributeExtension
{
    /// <summary>
    /// Gets or sets the maximum raw input value used for signal conversion and scaling.
    /// </summary>
    [GalaxyAttribute("RawMax", Security.Configure)]
    public AttributeConfig? RawMax { get; init; }

    /// <summary>
    /// Gets or sets the minimum raw input value used for signal conversion and scaling.
    /// </summary>
    [GalaxyAttribute("RawMin", Security.Configure)]
    public AttributeConfig? RawMin { get; init; }

    /// <summary>
    /// Gets or sets the maximum engineering units value corresponding to <see cref="RawMax" />.
    /// </summary>
    [GalaxyAttribute("EngUnitsMax", Security.Configure)]
    public AttributeConfig? EngMax { get; init; }

    /// <summary>
    /// Gets or sets the minimum engineering units value corresponding to <see cref="RawMin" />.
    /// </summary>
    [GalaxyAttribute("EngUnitsMin", Security.Configure)]
    public AttributeConfig? EngMin { get; init; }

    /// <summary>
    /// Gets or sets the maximum value of the engineering units range.
    /// </summary>
    [GalaxyAttribute("EngUnitsRangeMax", Security.Configure)]
    public AttributeConfig? RangeMax { get; init; }

    /// <summary>
    /// Gets or sets the minimum value of the engineering units range.
    /// </summary>
    [GalaxyAttribute("EngUnitsRangeMin", Security.Configure)]
    public AttributeConfig? RangeMin { get; init; }

    /// <summary>
    /// Gets or sets the conversion mode used for scaling raw values to engineering units.
    /// </summary>
    [GalaxyAttribute("ConversionMode")]
    public AttributeConfig? ConversionMode { get; init; }

    /// <summary>
    /// Gets or sets the configuration indicating whether clamping to engineering unit range limits is enabled.
    /// </summary>
    [GalaxyAttribute("ClampEnabled", Security.Configure)]
    public AttributeConfig? ClampEnabled { get; init; }

    public bool Supports(DataType dataType)
    {
        return dataType.IsNumeric();
    }

    /// <inheritdoc />
    public ScalingExtension Merge(ScalingExtension? current)
    {
        if (current is null) return this;

        return new ScalingExtension
        {
            RawMax = RawMax?.Merge(current.RawMax) ?? current.RawMax,
            RawMin = RawMin?.Merge(current.RawMin) ?? current.RawMin,
            EngMax = EngMax?.Merge(current.EngMax) ?? current.EngMax,
            EngMin = EngMin?.Merge(current.EngMin) ?? current.EngMin,
            RangeMax = RangeMax?.Merge(current.RangeMax) ?? current.RangeMax,
            RangeMin = RangeMin?.Merge(current.RangeMin) ?? current.RangeMin,
            ConversionMode = ConversionMode?.Merge(current.ConversionMode) ?? current.ConversionMode,
            ClampEnabled = ClampEnabled?.Merge(current.ClampEnabled ?? current.ClampEnabled)
        };
    }
}