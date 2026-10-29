using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Extensions;
using StarGate.Galaxy.Abstractions;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Handlers;

public class LimitAlarmExtensionHandler : IConfigurationHandler<LimitAlarmExtension>
{
    private static readonly AnalogAlarmExtensionHandler AnalogAlarmHandler = new();

    public LimitAlarmExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(LimitAlarmExtension), context))
            return null;

        // This extension requires a subproperty to be checked for the attribute to be extended.
        if (!target.TryGetAttribute($"{context}.LimitAlarmed", out var control) || !control.value.GetBoolean())
            return null;

        return new LimitAlarmExtension
        {
            HiHi = AnalogAlarmHandler.Extract(target, $"{context}.HiHi"),
            Hi = AnalogAlarmHandler.Extract(target, $"{context}.Hi"),
            Lo = AnalogAlarmHandler.Extract(target, $"{context}.Lo"),
            LoLo = AnalogAlarmHandler.Extract(target, $"{context}.LoLo"),
            Deadband = target.GetConfig<LimitAlarmExtension>(x => x.Deadband, context),
            Delay = target.GetConfig<LimitAlarmExtension>(x => x.Delay, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, LimitAlarmExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrEmpty(context) || source is null)
            return GalaxyResult.Ok();
        
        // Ensure the host attribute exists and supports this extension type.
        if (!target.ValidateExtension(source, context, out var invalid))
            return invalid;

        var result = target.EnableAttribute($"{context}.LimitAlarmed")
            .Then(() => AnalogAlarmHandler.Apply(target, source.HiHi, $"{context}.HiHi"))
            .And(() => AnalogAlarmHandler.Apply(target, source.Hi, $"{context}.Hi"))
            .And(() => AnalogAlarmHandler.Apply(target, source.Lo, $"{context}.Lo"))
            .And(() => AnalogAlarmHandler.Apply(target, source.LoLo, $"{context}.LoLo"))
            .And(() => target.Configure(source, x => x.Deadband, context))
            .And(() => target.Configure(source, x => x.Delay, context));

        return result;
    }
}