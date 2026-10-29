namespace StarGate.Core.Extensions;

/// <summary>
/// 
/// </summary>
public class GraphicExtension
{
    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string VisualContent { get; init; } = string.Empty;
}