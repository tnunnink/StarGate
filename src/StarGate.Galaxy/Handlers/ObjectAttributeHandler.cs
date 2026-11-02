using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

public class ObjectAttributeHandler : IConfigurationHandler<ObjectAttribute>
{
    private readonly AlarmExtensionHandler _alarmHandler = new();
    private readonly HistoryExtensionHandler _historyHandler = new();
    private readonly IOExtensionHandler _ioHandler = new();
    private readonly LoggingExtensionHandler _loggingHandler = new();

    /// <inheritdoc />
    public ObjectAttribute? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.TryGetAttribute(context, out var uda))
            return null;

        return new ObjectAttribute
        {
            Name = uda.Name,
            DataType = uda.DataType.ToCoreType(),
            Length = uda.GetLength(),
            Writeability = uda.GetWriteability(),
            Value = target.GetConfig<ObjectAttribute>(x => x.Value, context),
            Description = target.GetConfig<ObjectAttribute>(x => x.Description, context),
            Units = target.GetConfig<ObjectAttribute>(x => x.Units, context),
            IO = _ioHandler.Extract(target, context),
            History = _historyHandler.Extract(target, context),
            Log = _loggingHandler.Extract(target, context),
            Alarms = _alarmHandler.Extract(target, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, ObjectAttribute? source, string? context = null)
    {
        if (source is null) return GalaxyResult.Ok();

        var current = Extract(target, source.Name);
        var merged = source.Merge(current);

        return target.ReplaceAttribute(merged)
            .Then(() => target.Configure(merged, x => x.Value, source.Name))
            .And(() => target.Configure(merged, x => x.Description, source.Name))
            .And(() => target.Configure(merged, x => x.Units, source.Name))
            .And(() => _ioHandler.Apply(target, merged.IO, source.Name))
            .And(() => _historyHandler.Apply(target, merged.History, source.Name))
            .And(() => _loggingHandler.Apply(target, merged.Log, source.Name))
            .And(() => _alarmHandler.Apply(target, merged.Alarms, source.Name));
    }
}