using ArchestrA.GRAccess;
using StarGate.Core;

namespace StarGate.Galaxy;

public class QualityAlarmExtensionHandler : IConfigurationHandler<QualityAlarmExtension>
{
    /// <inheritdoc />
    public QualityAlarmExtension? Extract(IgObject target, string? context = null)
    {
        if (context is null || string.IsNullOrWhiteSpace(context))
            return null;

        if (!target.HasExtension(typeof(QualityAlarmExtension), context))
            return null;

        return new QualityAlarmExtension
        {
            Message = target.GetConfig<QualityAlarmExtension>(x => x.Message, context),
            Priority = target.GetConfig<QualityAlarmExtension>(x => x.Priority, context)
        };
    }

    public GalaxyResult Apply(IgObject target, QualityAlarmExtension? source, string? context = null)
    {
        if (context is null || string.IsNullOrEmpty(context) || source is null)
            return GalaxyResult.Ok();

        // Ensure the host attribute exists and supports this extension type.
        if (!target.ValidateExtension(source, context, out var invalid))
            return invalid;

        var result = target.AddExtension<QualityAlarmExtension>(context)
            .Then(() => target.Configure(source, x => x.Message, context))
            .Then(() => target.Configure(source, x => x.Priority, context));

        return result;
    }
}