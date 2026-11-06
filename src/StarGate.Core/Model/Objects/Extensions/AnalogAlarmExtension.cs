namespace StarGate.Core;

/// <summary>
/// Represents an extension for managing analog alarm-specific configurations.
/// This class is capable of merging with another instance to combine or override
/// property values as needed for configuration purposes.
/// </summary>
public class AnalogAlarmExtension
{
    /// <summary>
    /// Represents the configuration for the "Limit" attribute in the AnalogAlarmExtension.
    /// The "Limit" specifies a value or constraint associated with the alarm and determines its behavior or threshold.
    /// This property is decorated with the <see cref="GalaxyAttributeAttribute" /> indicating its metadata and default
    /// security level.
    /// </summary>
    //todo this won't work for deviation alarms because it is called tolerance...
    [GalaxyAttribute("Limit", Security.Tune)]
    public AttributeConfig? Limit { get; init; }

    /// <summary>
    /// Represents the configuration for the "Priority" attribute in the AnalogAlarmExtension.
    /// The "Priority" determines the hierarchical importance or urgency level assigned to the alarm
    /// and influences its behavior or processing order.
    /// </summary>
    [GalaxyAttribute("Priority", Security.Tune)]
    public AttributeConfig? Priority { get; init; }

    /// <summary>
    /// Represents the configuration for the "Message" attribute in the AnalogAlarmExtension.
    /// The "Message" provides a description or metadata associated with the analog alarm.
    /// </summary>
    [GalaxyAttribute("DescAttrName")]
    public AttributeConfig? Message { get; init; }
}