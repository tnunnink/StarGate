using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Extensions;
using StarGate.Galaxy.Abstractions;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Handlers;

/// <summary>
/// Handles the extraction and application of the HistoryExtension configuration
/// on Galaxy objects. Implements the <see cref="IConfigurationHandler{T}"/> interface
/// to enable management of HistoryExtension data for assigned objects.
/// </summary>
/// <remarks>
/// The HistoryExtensionHandler is responsible for interacting with the HistoryExtension
/// settings of Galaxy objects by extracting their configuration details and applying
/// changes as needed. It ensures compatibility with the structure of ArchestrA Galaxy Runtime
/// while working with the history-related properties.
/// </remarks>
public class HistoryExtensionHandler : IConfigurationHandler<HistoryExtension>
{
    public HistoryExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(HistoryExtension), context))
            return null;

        return new HistoryExtension
        {
            Description = target.GetConfig<HistoryExtension>(x => x.Description, context),
            StoragePeriod = target.GetConfig<HistoryExtension>(x => x.StoragePeriod, context),
            Deadband = target.GetConfig<HistoryExtension>(x => x.Deadband, context),
            TrendHi = target.GetConfig<HistoryExtension>(x => x.TrendHi, context),
            TrendLo = target.GetConfig<HistoryExtension>(x => x.TrendLo, context),
            SwingDoorEnabled = target.GetConfig<HistoryExtension>(x => x.SwingDoorEnabled, context),
            InterpolationType = target.GetConfig<HistoryExtension>(x => x.InterpolationType, context),
            Rollover = target.GetConfig<HistoryExtension>(x => x.Rollover, context)
        };
    }

    public GalaxyResult Apply(IgObject target, HistoryExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrEmpty(context) || source is null)
            return GalaxyResult.Ok();

        // Ensure the host attribute exists and supports this extension type.
        if (!target.ValidateExtension(source, context, out var invalid))
            return invalid;

        var result = target.AddExtension<HistoryExtension>(context)
            .Then(() => target.Configure(source, x => x.Description, context))
            .And(() => target.Configure(source, x => x.StoragePeriod, context))
            .And(() => target.Configure(source, x => x.Deadband, context))
            .And(() => target.Configure(source, x => x.TrendHi, context))
            .And(() => target.Configure(source, x => x.TrendLo, context))
            .And(() => target.Configure(source, x => x.SwingDoorEnabled, context))
            .And(() => target.Configure(source, x => x.InterpolationType, context))
            .And(() => target.Configure(source, x => x.Rollover, context));

        return result;
    }
}