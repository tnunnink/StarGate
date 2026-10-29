using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Extensions;
using StarGate.Galaxy.Abstractions;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Handlers;

public class RocAlarmExtensionHandler : IConfigurationHandler<RocAlarmExtension>
{
    private static readonly AnalogAlarmExtensionHandler AnalogAlarmHandler = new();

    public RocAlarmExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(DeviationAlarmExtension), context))
            return null;

        // This extension requires a subproperty to be checked for the attribute to be extended.
        if (!target.TryGetAttribute($"{context}.RocAlarmed", out var control) || !control.value.GetBoolean())
            return null;

        return new RocAlarmExtension
        {
            Up = AnalogAlarmHandler.Extract(target, $"{context}.Dev.Major"),
            Down = AnalogAlarmHandler.Extract(target, $"{context}.Dev.Minor"),
            Period = target.GetConfig<RocAlarmExtension>(x => x.Period, context),
            Units = target.GetConfig<RocAlarmExtension>(x => x.Units, context),
        };
    }

    public GalaxyResult Apply(IgObject target, RocAlarmExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrEmpty(context) || source is null)
            return GalaxyResult.Ok();

        // Ensure the host attribute exists and supports this extension type.
        if (!target.ValidateExtension(source, context, out var invalid))
            return invalid;

        var result = target.AddExtension<RocAlarmExtension>(context)
            .Then(() => target.EnableAttribute($"{context}.RocAlarmed"))
            .Then(() => AnalogAlarmHandler.Apply(target, source.Up, $"{context}.Roc.IncreasingHi"))
            .And(() => AnalogAlarmHandler.Apply(target, source.Down, $"{context}.Roc.DecreasingHi"))
            .And(() => target.Configure(source, x => x.Period, context))
            .And(() => target.Configure(source, x => x.Units, context));

        return result;
    }
}