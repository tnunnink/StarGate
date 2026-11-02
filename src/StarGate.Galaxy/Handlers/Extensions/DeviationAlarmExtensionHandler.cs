using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

public class DeviationAlarmExtensionHandler : IConfigurationHandler<DeviationAlarmExtension>
{
    private static readonly AnalogAlarmExtensionHandler AnalogAlarmHandler = new();

    public DeviationAlarmExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(DeviationAlarmExtension), context))
            return null;

        // This extension requires a subproperty to be checked for the attribute to be extended.
        if (!target.TryGetAttribute($"{context}.DeviationAlarmed", out var control) || !control.value.GetBoolean())
            return null;

        return new DeviationAlarmExtension
        {
            Major = AnalogAlarmHandler.Extract(target, $"{context}.Dev.Major"),
            Minor = AnalogAlarmHandler.Extract(target, $"{context}.Dev.Minor"),
            Target = target.GetConfig<DeviationAlarmExtension>(x => x.Target, context),
            Deadband = target.GetConfig<DeviationAlarmExtension>(x => x.Deadband, context),
            SettlingPeriod = target.GetConfig<DeviationAlarmExtension>(x => x.SettlingPeriod, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, DeviationAlarmExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrEmpty(context) || source is null)
            return GalaxyResult.Ok();

        // Ensure the host attribute exists and supports this extension type.
        if (!target.ValidateExtension(source, context, out var invalid))
            return invalid;

        var result = target.AddExtension<DeviationAlarmExtension>(context)
            .Then(() => target.EnableAttribute($"{context}.DeviationAlarmed"))
            .Then(() => AnalogAlarmHandler.Apply(target, source.Major, $"{context}.Dev.Major"))
            .And(() => AnalogAlarmHandler.Apply(target, source.Minor, $"{context}.Dev.Minor"))
            .And(() => target.Configure(source, x => x.Target, context))
            .And(() => target.Configure(source, x => x.Deadband, context))
            .And(() => target.Configure(source, x => x.SettlingPeriod, context));

        return result;
    }
}