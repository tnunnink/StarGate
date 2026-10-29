namespace StarGate.Core.Graphics.Styling;

/// <summary>
/// Specifies how a texture or pattern is applied to a surface when the texture size does
/// not match the dimensions of the surface. This can determine stretching behavior or
/// tiling repetition of the texture.
/// </summary>
public enum WrapMode
{
    Stretch,
    Tile,
}