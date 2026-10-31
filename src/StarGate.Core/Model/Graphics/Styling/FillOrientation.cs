namespace StarGate.Core;

/// <summary>
/// Specifies the orientation for filling graphical content.
/// This can be relative to the graphic itself or relative to the screen.
/// </summary>
public enum FillOrientation
{
    /// <summary>
    /// Specifies that the filling orientation should be defined relative to the graphic itself.
    /// When this option is selected, the fill orientation aligns based on the orientation
    /// and properties of the particular graphic element being rendered.
    /// </summary>
    RelativeToGraphic,

    /// <summary>
    /// Specifies that the filling orientation should be determined relative to the screen.
    /// When this option is selected, the fill aligns based on the screen's orientation
    /// and coordinate system, independent of the specific graphic element being rendered.
    /// </summary>
    RelativeToScreen
}