using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Extensions;
using StarGate.Galaxy.Abstractions;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Handlers;

/// <summary>
/// Handles the extraction and application of configuration data for
/// the <see cref="StateAlarmExtension"/> type in Galaxy objects.
/// Implements the <see cref="IConfigurationHandler{T}"/> interface specifically
/// for the <see cref="StateAlarmExtension"/> type.
/// </summary>
public class StateAlarmExtensionHandler : IConfigurationHandler<StateAlarmExtension>
{
    /// <inheritdoc />
    public StateAlarmExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(StateAlarmExtension), context))
            return null;

        return new StateAlarmExtension
        {
            Category = target.GetConfig<StateAlarmExtension>(x => x.Category, context),
            Priority = target.GetConfig<StateAlarmExtension>(x => x.Priority, context),
            Message = target.GetConfig<StateAlarmExtension>(x => x.Message, context),
            ActiveState = target.GetConfig<StateAlarmExtension>(x => x.ActiveState, context),
            Source = target.GetConfig<StateAlarmExtension>(x => x.Source, context),
            Delay = target.GetConfig<StateAlarmExtension>(x => x.Delay, context)
        };
    }

    /// <inheritdoc />
    public GalaxyResult Apply(IgObject target, StateAlarmExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrEmpty(context) || source is null)
            return GalaxyResult.Ok();
        
        // Ensure the host attribute exists and supports this extension type.
        if (!target.ValidateExtension(source, context, out var invalid))
            return invalid;

        var result = target.AddExtension<StateAlarmExtension>(context)
            .Then(() => target.Configure(source, x => x.Category, context))
            .And(() => target.Configure(source, x => x.Priority, context))
            .And(() => target.Configure(source, x => x.Message, context))
            .And(() => target.Configure(source, x => x.ActiveState, context))
            .And(() => target.Configure(source, x => x.Source, context))
            .And(() => target.Configure(source, x => x.Delay, context));

        return result;
    }
}