namespace StarGate.Core;

/// <summary>
/// Extension that defines the deviation alarm configuration (major and minor) for an analog Galaxy attribute, measured
/// against a target value.
/// </summary>
[GalaxyExtension("AnalogExtension", ExtensionType.Attribute)]
public class DeviationAlarmExtension : IAttributeExtension
{
    /// <summary>
    /// The major deviation alarm configuration, raised when the value deviates far from the target.
    /// </summary>
    public AnalogAlarmExtension? Major { get; init; }

    /// <summary>
    /// The minor deviation alarm configuration, raised when the value deviates slightly from the target.
    /// </summary>
    public AnalogAlarmExtension? Minor { get; init; }

    /// <summary>
    /// The target (setpoint) value that deviation is measured against.
    /// </summary>
    [GalaxyAttribute("Dev.Target", Security.Operate)]
    public AttributeConfig? Target { get; init; }

    /// <summary>
    /// The value deadband applied to deviation alarms, preventing alarm chatter near a threshold.
    /// </summary>
    [GalaxyAttribute("Dev.Deadband", Security.Tune)]
    public AttributeConfig? Deadband { get; init; }

    /// <summary>
    /// The settling period after a target change during which deviation alarms are suppressed.
    /// </summary>
    [GalaxyAttribute("Dev.SettlingPeriod", Security.Tune)]
    public AttributeConfig? SettlingPeriod { get; init; }

    /// <inheritdoc />
    public bool Supports(AttributeDataType dataType)
    {
        return dataType.IsNumeric();
    }
}