namespace StarGate.Core;

/// <summary>
/// Identifies the category of graphic element, shape, text object, embedded content, connector, or interactive control.
/// </summary>
/// <remarks>
/// Used to distinguish concrete element types when processing, rendering, serializing, or binding graphic elements.
/// </remarks>
public enum ElementKind
{
    Line,
    Polyline,
    Rectangle,
    Ellipse,
    Polygon,
    Curve,
    ClosedCurve,
    Text,
    TextBox,
    Button,
    Image,
    Pie,
    Arc,
    Chord,
    Group,
    Path,
    EmbeddedSymbol,
    Status,
    CheckBox,
    EditBox,
    DateTimePicker,
    Calendar,
    ComboBox,
    ListBox,
    RadioButton,
    TrendPen,
    EmbeddedClientControl,
    HtmlWidget,
    Connector,
    ConnectionPoint,
    TrendPens
}