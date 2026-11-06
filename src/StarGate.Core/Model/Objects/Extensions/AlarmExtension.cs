namespace StarGate.Core;

/// <summary>
/// Represents an alarm extension that groups the different alarm types which can be
/// configured on an attribute: state, limit, rate of change, deviation, and quality.
/// </summary>
public class AlarmExtension
{
    /// <summary>
    /// Gets the state (discrete) alarm configuration, which alarms when the value matches a specific state.
    /// </summary>
    public StateAlarmExtension? State { get; init; }

    /// <summary>
    /// Gets the limit alarm configuration, which alarms when the value crosses configured high or low limits.
    /// </summary>
    public LimitAlarmExtension? Limit { get; init; }

    /// <summary>
    /// Gets the rate of change alarm configuration, which alarms when the value changes faster than a configured rate.
    /// </summary>
    public RocAlarmExtension? Roc { get; init; }

    /// <summary>
    /// Gets the deviation alarm configuration, which alarms when the value strays too far from a target value.
    /// </summary>
    public DeviationAlarmExtension? Deviation { get; init; }

    /// <summary>
    /// Gets the quality alarm configuration, which alarms when the value's quality becomes bad or uncertain.
    /// </summary>
    public QualityAlarmExtension? Quality { get; init; }
}