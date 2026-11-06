using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

public class ObjectAttributeHandler : IConfigurationHandler<GalaxyAttribute>
{
    private readonly AlarmExtensionHandler _alarmHandler = new();
    private readonly HistoryExtensionHandler _historyHandler = new();
    private readonly IOExtensionHandler _ioHandler = new();
    private readonly LoggingExtensionHandler _loggingHandler = new();

    /// <inheritdoc />
    public GalaxyAttribute? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.TryGetAttribute(context, out var uda))
            return null;

        return new GalaxyAttribute
        {
            Name = uda.Name,
            DataType = uda.DataType.ToCoreType(),
            Length = uda.GetLength(),
            Writeability = uda.GetWriteability(),
            Value = target.GetConfig<GalaxyAttribute>(x => x.Value, context),
            Description = target.GetConfig<GalaxyAttribute>(x => x.Description, context),
            Units = target.GetConfig<GalaxyAttribute>(x => x.Units, context),
            IO = _ioHandler.Extract(target, context),
            History = _historyHandler.Extract(target, context),
            Log = _loggingHandler.Extract(target, context),
            Alarms = _alarmHandler.Extract(target, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, GalaxyAttribute? source, string? context = null)
    {
        if (source is null) return GalaxyResult.Ok();

        return target.ReplaceAttribute(source)
            .Then(() => target.Configure(source, x => x.Value, source.Name))
            .And(() => target.Configure(source, x => x.Description, source.Name))
            .And(() => target.Configure(source, x => x.Units, source.Name))
            .And(() => _ioHandler.Apply(target, source.IO, source.Name))
            .And(() => _historyHandler.Apply(target, source.History, source.Name))
            .And(() => _loggingHandler.Apply(target, source.Log, source.Name))
            .And(() => _alarmHandler.Apply(target, source.Alarms, source.Name));
    }
}