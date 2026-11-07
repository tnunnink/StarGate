namespace StarGate.Core;

/// <summary>
/// Represents an alarm extension that encapsulates the configuration attributes used to
/// define an alarm, such as its category, priority, message, source, and time deadband.
/// </summary>
[GalaxyExtension("AlarmExtension", ExtensionType.Attribute)]
public class StateAlarmExtension : IAttributeExtension
{
    /// <summary>
    /// Gets the configuration for the alarm category, which classifies the alarm
    /// (for example, discrete, limit, or system).
    /// </summary>
    [GalaxyAttribute("Category", Security.Tune)]
    public AttributeConfig? Category { get; init; }

    /// <summary>
    /// Gets the configuration for the alarm priority, which indicates its severity or importance.
    /// </summary>
    [GalaxyAttribute("Priority", Security.Tune)]
    public AttributeConfig? Priority { get; init; }

    /// <summary>
    /// Gets the configuration for the name of the attribute whose value is used as the
    /// alarm message or description.
    /// </summary>
    [GalaxyAttribute("DescAttrName", Security.FreeAccess)]
    public AttributeConfig? Message { get; init; }

    /// <summary>
    /// Gets the configuration for the attribute or reference that determines whether the
    /// alarm is in its active state.
    /// </summary>
    [GalaxyAttribute("ActiveAlarmState")]
    public AttributeConfig? ActiveState { get; init; }

    /// <summary>
    /// Gets the configuration for the alarm source attribute, identifying the attribute
    /// reported as the source of the alarm.
    /// </summary>
    [GalaxyAttribute("AlarmSourceAttr", Security.Configure)]
    public AttributeConfig? Source { get; init; }

    /// <summary>
    /// Gets the configuration for the time deadband, the duration a condition must persist
    /// before the alarm is raised.
    /// </summary>
    [GalaxyAttribute("Alarm.TimeDeadband", Security.Tune)]
    public AttributeConfig? Delay { get; init; }

    /// <inheritdoc />
    public bool Supports(AttributeDataType dataType)
    {
        return dataType == AttributeDataType.Boolean;
    }
}