using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

public class AnalogAlarmExtensionHandler : IConfigurationHandler<AnalogAlarmExtension>
{
    /// <inheritdoc />
    public AnalogAlarmExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        // This extension requires a subproperty to be checked for the attribute to be extended.
        // Fortunately, all analog alarm extended properties follow this convention of "{primitive_name}.{alarm_name}.Alarmed"
        // In this case context is the path to the alarm root. Check if it exists and is enabled before reading attributes.
        if (!target.TryGetAttribute($"{context}.Alarmed", out var control) || !control.value.GetBoolean())
            return null;

        return new AnalogAlarmExtension
        {
            Limit = target.GetConfig<AnalogAlarmExtension>(x => x.Limit, context),
            Priority = target.GetConfig<AnalogAlarmExtension>(x => x.Priority, context),
            Message = target.GetConfig<AnalogAlarmExtension>(x => x.Message, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, AnalogAlarmExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context) || source is null)
            return GalaxyResult.Ok();

        return target.EnableAttribute($"{context}.Alarmed")
            .Then(() => target.Configure(source, x => x.Limit, context))
            .And(() => target.Configure(source, x => x.Priority, context))
            .And(() => target.Configure(source, x => x.Message, context));
    }
}