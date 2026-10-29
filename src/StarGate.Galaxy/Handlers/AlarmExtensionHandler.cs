using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Extensions;
using StarGate.Galaxy.Abstractions;

namespace StarGate.Galaxy.Handlers;

public class AlarmExtensionHandler : IConfigurationHandler<AlarmExtension>
{
    private static readonly StateAlarmExtensionHandler StateHandler = new();
    private static readonly LimitAlarmExtensionHandler LimitHandler = new();
    private static readonly RocAlarmExtensionHandler RocHandler = new();
    private static readonly DeviationAlarmExtensionHandler DeviationHandler = new();
    private static readonly QualityAlarmExtensionHandler QualityHandler = new();

    /// <inheritdoc />
    public AlarmExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        return new AlarmExtension
        {
            State = StateHandler.Extract(target, context),
            Limit = LimitHandler.Extract(target, context),
            Roc = RocHandler.Extract(target, context),
            Deviation = DeviationHandler.Extract(target, context),
            Quality = QualityHandler.Extract(target, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, AlarmExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrEmpty(context) || source is null)
            return GalaxyResult.Ok();

        return StateHandler.Apply(target, source.State, context)
            .And(() => LimitHandler.Apply(target, source.Limit, context))
            .And(() => RocHandler.Apply(target, source.Roc, context))
            .And(() => DeviationHandler.Apply(target, source.Deviation, context))
            .And(() => QualityHandler.Apply(target, source.Quality, context));
    }
}