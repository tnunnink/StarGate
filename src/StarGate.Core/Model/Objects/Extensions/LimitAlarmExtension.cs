namespace StarGate.Core;

/// <summary>
/// Extension that defines the four-level limit alarm configuration (HiHi, Hi, Lo, LoLo) for an analog Galaxy
/// attribute,
/// including the shared deadband and time delay settings.
/// </summary>
[GalaxyExtension("AnalogExtension", ExtensionType.Attribute)]
public class LimitAlarmExtension : IGalaxyMergeable<LimitAlarmExtension>, IAttributeExtension
{
    /// <summary>
    /// The high-high (most severe high-limit) alarm configuration.
    /// </summary>
    public AnalogAlarmExtension? HiHi { get; init; }

    /// <summary>
    /// The high-limit alarm configuration.
    /// </summary>
    public AnalogAlarmExtension? Hi { get; init; }

    /// <summary>
    /// The low-limit alarm configuration.
    /// </summary>
    public AnalogAlarmExtension? Lo { get; init; }

    /// <summary>
    /// The low-low (most severe low limit) alarm configuration.
    /// </summary>
    public AnalogAlarmExtension? LoLo { get; init; }

    /// <summary>
    /// The value deadband applied to all limit alarms, preventing alarm chatter near a limit.
    /// </summary>
    [GalaxyAttribute("LevelAlarm.ValueDeadband", Security.Tune)]
    public AttributeConfig? Deadband { get; init; }

    /// <summary>
    /// The time deadband (delay) a limit must be violated before the alarm is raised.
    /// </summary>
    [GalaxyAttribute("LevelAlarm.TimeDeadband", Security.Tune)]
    public AttributeConfig? Delay { get; init; }

    /// <inheritdoc />
    public bool Supports(DataType dataType)
    {
        return dataType.IsNumeric();
    }

    /// <inheritdoc />
    public LimitAlarmExtension Merge(LimitAlarmExtension? current)
    {
        if (current is null) return this;

        return new LimitAlarmExtension
        {
            HiHi = HiHi?.Merge(current.HiHi) ?? current.HiHi,
            Hi = Hi?.Merge(current.Hi) ?? current.Hi,
            Lo = Lo?.Merge(current.Lo) ?? current.Lo,
            LoLo = LoLo?.Merge(current.LoLo) ?? current.LoLo,
            Deadband = Deadband?.Merge(current.Deadband) ?? current.Deadband,
            Delay = Delay?.Merge(current.Delay) ?? current.Delay
        };
    }
}