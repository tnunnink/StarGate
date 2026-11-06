namespace StarGate.Core;

/// <summary>
/// Represents a rate of change (ROC) alarm extension that alarms when a value increases or
/// decreases faster than a configured rate over an evaluation period.
/// </summary>
[GalaxyExtension("AnalogExtension", ExtensionType.Attribute)]
public class RocAlarmExtension : IAttributeExtension
{
    /// <summary>
    /// Gets the alarm configuration that triggers when the value is increasing faster than the configured rate.
    /// </summary>
    [GalaxyAttribute("Roc.IncreasingHi.Alarmed")]
    public AnalogAlarmExtension? Up { get; init; }

    /// <summary>
    /// Gets the alarm configuration that triggers when the value is decreasing faster than the configured rate.
    /// </summary>
    [GalaxyAttribute("Roc.IncreasingLo.Alarmed")]
    public AnalogAlarmExtension? Down { get; init; }

    /// <summary>
    /// Gets the configuration for the evaluation period over which the rate of change is calculated.
    /// </summary>
    [GalaxyAttribute("Roc.EvalPeriod", Security.Tune)]
    public AttributeConfig? Period { get; init; }

    /// <summary>
    /// Gets the configuration for the engineering units in which the rate of change is expressed.
    /// </summary>
    [GalaxyAttribute("Roc.Rate.Units")]
    public AttributeConfig? Units { get; init; }

    /// <inheritdoc />
    public bool Supports(DataType dataType)
    {
        return dataType.IsNumeric();
    }
}