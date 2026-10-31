namespace StarGate.Core;

/// <summary>
/// Represents a history extension that encapsulates various configuration attributes
/// for historical data management. This class allows the configuration of different
/// behaviors such as data interpolation, deadband, storage period controls, and more.
/// </summary>
[GalaxyExtension("HistoryExtension", ExtensionType.Attribute)]
public class HistoryExtension : IGalaxyMergeable<HistoryExtension>, IAttributeExtension
{
    /// <summary>
    /// Gets the configuration for the name of the attribute whose value is used as the
    /// description of the historized tag.
    /// </summary>
    [GalaxyAttribute("Hist.DescAttrName", Security.Operate)]
    public AttributeConfig? Description { get; init; }

    /// <summary>
    /// Gets the configuration for the maximum time interval between stored values, forcing
    /// a value to be stored even when it has not changed.
    /// </summary>
    [GalaxyAttribute("ForceStoragePeriod", Security.Configure)]
    public AttributeConfig? StoragePeriod { get; init; }

    /// <summary>
    /// Gets the configuration for the value deadband, the minimum change in value required
    /// before a new value is stored.
    /// </summary>
    [GalaxyAttribute("ValueDeadBand", Security.Configure)]
    public AttributeConfig? Deadband { get; init; }

    /// <summary>
    /// Gets the configuration for the upper limit of the default trend display range.
    /// </summary>
    [GalaxyAttribute("TrendHi", Security.Operate)]
    public AttributeConfig? TrendHi { get; init; }

    /// <summary>
    /// Gets the configuration for the lower limit of the default trend display range.
    /// </summary>
    [GalaxyAttribute("TrendLo", Security.Operate)]
    public AttributeConfig? TrendLo { get; init; }

    /// <summary>
    /// Gets the configuration that enables swinging door compression, which stores values
    /// only when they deviate from the projected trend beyond the deadband.
    /// </summary>
    [GalaxyAttribute("EnableSwingingDoor", Security.Operate)]
    public AttributeConfig? SwingDoorEnabled { get; init; }

    /// <summary>
    /// Gets the configuration for the interpolation type used to retrieve values between
    /// stored history points.
    /// </summary>
    [GalaxyAttribute("InterpolationType", Security.Operate)]
    public AttributeConfig? InterpolationType { get; init; }

    /// <summary>
    /// Gets the configuration for the rollover value, the point at which a counter-style
    /// value wraps back to zero so history can handle the rollover correctly.
    /// </summary>
    [GalaxyAttribute("RolloverValue", Security.Operate)]
    public AttributeConfig? Rollover { get; init; }

    /// <inheritdoc />
    public bool Supports(DataType dataType)
    {
        return dataType is not DataType.InternationalizedString and not DataType.Time;
    }

    /// <inheritdoc />
    public HistoryExtension Merge(HistoryExtension? current)
    {
        if (current is null) return this;

        return new HistoryExtension
        {
            Description = Description?.Merge(current.Description) ?? current.Description,
            StoragePeriod = StoragePeriod?.Merge(current.StoragePeriod) ?? current.StoragePeriod,
            Deadband = Deadband?.Merge(current.Deadband) ?? current.Deadband,
            TrendHi = TrendHi?.Merge(current.TrendHi) ?? current.TrendHi,
            TrendLo = TrendLo?.Merge(current.TrendLo) ?? current.TrendLo,
            SwingDoorEnabled = SwingDoorEnabled?.Merge(current.SwingDoorEnabled) ?? current.SwingDoorEnabled,
            InterpolationType = InterpolationType?.Merge(current.InterpolationType) ?? current.InterpolationType,
            Rollover = Rollover?.Merge(current.Rollover) ?? current.Rollover
        };
    }
}