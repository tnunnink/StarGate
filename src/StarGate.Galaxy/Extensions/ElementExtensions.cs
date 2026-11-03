using System;
using System.Xml.Linq;
using StarGate.Core;

namespace StarGate.Galaxy;

/// <summary>
/// Provides extension methods for <see cref="XElement" /> instances.
/// </summary>
public static class ElementExtensions
{
    /// <param name="element">The <see cref="XElement" /> to retrieve the attribute value from.</param>
    extension(XElement element)
    {
        /// <summary>
        /// Retrieves a child XML element from the current element based on the specified selector path.
        /// </summary>
        /// <param name="selector">
        /// A dot-separated string specifying the path of child elements to locate. Each segment represents a child element
        /// in the XML hierarchy. If the path is invalid or no matching element is found, null is returned.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/> that matches the specified selector, or null if no matching element exists.
        /// </returns>
        public XElement? GetChild(string selector)
        {
            return element.SelectElement(selector);
        }

        /// <summary>
        /// Retrieves the value of an attribute or element specified by the selector.
        /// Throws an <see cref="InvalidOperationException"/> if the value is not found.
        /// </summary>
        /// <param name="selector">
        /// A string specifying the path of the attribute or element to locate.
        /// If the path is invalid or no matching value is found, an exception will be thrown.
        /// </param>
        /// <returns>
        /// The value of the specified attribute or element as a string.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the required value corresponding to the specified selector is not found.
        /// </exception>
        public string GetRequiredValue(string selector)
        {
            var value = element.SelectValue(selector);

            if (value is null)
                throw new InvalidOperationException(
                    $"Required attribute '{selector}' was not found on element '{element.Name}'.");

            return value;
        }

        /// <summary>
        /// Retrieves the value from the specified selector, applies a parser, and returns the required parsed value.
        /// Throws an exception if the selector does not yield a valid value.
        /// </summary>
        /// <param name="selector">
        /// Specifies the path or attribute to locate the value within the XML element. If it starts with '@', it represents an attribute;
        /// otherwise, it is treated as a descendant element selector or the current element's value if empty.
        /// </param>
        /// <param name="parser">A function to parse the extracted string into the desired type.</param>
        /// <typeparam name="TValue">The type of the value to be parsed and returned.</typeparam>
        /// <returns>The parsed value of type <typeparamref name="TValue"/> extracted from the selector.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified selector does not yield a valid value or the value is missing.
        /// </exception>
        public TValue GetRequiredValue<TValue>(string selector, Func<string, TValue> parser)
        {
            var value = element.SelectValue(selector);

            if (value is null)
                throw new InvalidOperationException(
                    $"Required attribute '{selector}' was not found on element '{element.Name}'.");

            return parser(value);
        }

        /// <summary>
        /// Retrieves the value associated with the specified selector from an XML element.
        /// The method can extract values from child elements or attributes based on the provided selector.
        /// </summary>
        /// <param name="selector">
        /// A string identifying the path to an XML element or an attribute. The path can be a series of dot-separated
        /// element names or an attribute name prefixed with '@'. If the selector is null or empty, the value of the
        /// current element is returned.
        /// </param>
        /// <returns>
        /// A string representing the value associated with the specified selector or null if the selector
        /// does not match any child element or attribute.
        /// </returns>
        public string? GetValue(string selector)
        {
            return element.SelectValue(selector);
        }

        /// <summary>
        /// Retrieves the value from the specified selector, applies a parser to it, and returns the parsed value.
        /// If the value is null or cannot be found, the provided fallback value is returned.
        /// </summary>
        /// <param name="selector">
        /// Specifies the path or attribute to retrieve the value from. If the selector starts with '@', it is treated as an attribute reference.
        /// Otherwise, it is treated as a descendant element selector or the current element's value if empty.
        /// </param>
        /// <param name="parser">A function to parse the retrieved string into the desired type.</param>
        /// <param name="fallback">The fallback value to return when the selector does not yield a valid value.</param>
        /// <typeparam name="TValue">The type of the value to be parsed and returned.</typeparam>
        /// <returns>The parsed value of type <typeparamref name="TValue"/>, or the fallback value if the selector does not match.</returns>
        public TValue? GetValue<TValue>(string selector, Func<string, TValue> parser, TValue? fallback = default)
        {
            var value = element.SelectValue(selector);
            return value is not null ? parser(value) : fallback;
        }

        /// <summary>
        /// Retrieves a value from the specified element using the given selector and parses it into a struct of the specified type.
        /// If the value is not found, the provided fallback value is returned.
        /// </summary>
        /// <typeparam name="TStruct">The type of the value to be returned, constrained to value types.</typeparam>
        /// <param name="selector">
        /// A string specifying how to locate the value. If it starts with '@', it is treated as an attribute of the current element.
        /// Otherwise, it is treated as a descendant element selector. An empty selector assumes the value of the current element.
        /// </param>
        /// <param name="parser">A function to parse the string into the specified struct type.</param>
        /// <param name="fallback">The fallback value to return if the specified value cannot be found or parsed.</param>
        /// <returns>
        /// The parsed value of type <typeparamref name="TStruct"/> if successful; otherwise, the fallback value is returned.
        /// </returns>
        public TStruct GetStruct<TStruct>(string selector, Func<string, TStruct> parser, TStruct fallback = default)
            where TStruct : struct
        {
            var value = element.SelectValue(selector);
            return value is not null ? parser(value) : fallback;
        }

        /// <summary>
        /// Retrieves the value of the specified selector as an enumeration of type <typeparamref name="TEnum"/>.
        /// </summary>
        /// <param name="selector">The name of the attribute or descendant element from which to retrieve the value.
        /// If the selector starts with '@', it is treated as an attribute reference; otherwise,
        /// it is treated as a descendant element.
        /// </param>
        /// <param name="fallback">The fallback value to return if the selector value is not found or cannot be parsed as <typeparamref name="TEnum"/>.</param>
        /// <typeparam name="TEnum">The enumeration type to which the value will be parsed. Must be a struct and an enumeration type.</typeparam>
        /// <returns>The parsed enumeration value of type <typeparamref name="TEnum"/> if successful, or the fallback value if parsing fails.</returns>
        public TEnum GetEnum<TEnum>(string selector, TEnum fallback = default) where TEnum : struct, Enum
        {
            var value = element.SelectValue(selector);

            if (value is not null && Enum.TryParse<TEnum>(value, true, out var result))
                return result;

            return fallback;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public TEnum? GetOptionalEnum<TEnum>(string selector) where TEnum : struct, Enum
        {
            var value = element.SelectValue(selector);

            if (value is not null && Enum.TryParse<TEnum>(value, true, out var result))
                return result;

            return null;
        }

        /// <summary>
        /// Retrieves the color information from the specified XML element and
        /// constructs a <see cref="GraphicColor" /> instance.
        /// </summary>
        /// <param name="selector">The name of the XML element containing the color attributes.</param>
        /// <returns>
        /// A <see cref="GraphicColor" /> instance representing the color defined in the XML.
        /// If the element or attributes are missing, returns the default color.
        /// </returns>
        public GraphicColor GetColor(string selector)
        {
            var color = element.SelectElement(selector);

            if (color is null) return default;

            var r = color.GetStruct("@R", byte.Parse);
            var g = color.GetStruct("@G", byte.Parse);
            var b = color.GetStruct("@B", byte.Parse);
            var a = color.GetStruct("@A", byte.Parse, (byte)255);

            return new GraphicColor(r, g, b, a);
        }

        /// <summary>
        /// Retrieves the graphical bounds of an element, including its position and dimensions.
        /// </summary>
        /// <returns>
        /// A <see cref="GraphicBounds" /> instance containing the left, top, width, and height
        /// values of the element obtained from the corresponding attributes.
        /// </returns>
        public GraphicBounds GetBounds()
        {
            return new GraphicBounds
            {
                Left = element.GetStruct("@Left", double.Parse),
                Top = element.GetStruct("@Top", double.Parse),
                Width = element.GetStruct("@Width", double.Parse),
                Height = element.GetStruct("@Height", double.Parse)
            };
        }

        /// <summary>
        /// Gets the origin point of the <see cref="XElement" /> as a <see cref="GraphicPoint" />.
        /// </summary>
        /// <returns>A <see cref="GraphicPoint" /> representing the origin of the element.
        /// If the "OriginX" or "OriginY" attributes are not present or cannot be parsed, their values default to 0.</returns>
        public GraphicPoint GetOrigin()
        {
            return new GraphicPoint
            {
                X = element.GetStruct("@OriginX", double.Parse),
                Y = element.GetStruct("@OriginY", double.Parse)
            };
        }

        /// <summary>
        /// Determines the flip state of a graphic element based on its transformation attributes
        /// in the <see cref="XElement" />.
        /// </summary>
        /// <returns>
        /// A <see cref="GraphicFlip" /> value that specifies whether the graphic element is flipped along
        /// the horizontal axis, vertical axis, both axes, or not flipped at all.
        /// </returns>
        public GraphicFlip GetFlip()
        {
            var hasTransform = element.Attribute("HasTransform")?.Value;

            if (!string.Equals(hasTransform, "true", StringComparison.OrdinalIgnoreCase))
                return GraphicFlip.None;

            var t0 = element.Attribute("Transform_0")?.Value;
            var t3 = element.Attribute("Transform_3")?.Value;

            var flipX = string.Equals(t0, "-1", StringComparison.Ordinal);
            var flipY = string.Equals(t3, "-1", StringComparison.Ordinal);

            return (flipX, flipY) switch
            {
                (true, true) => GraphicFlip.Both,
                (true, false) => GraphicFlip.Horizontal,
                (false, true) => GraphicFlip.Vertical,
                _ => GraphicFlip.None
            };
        }

        /// <summary>
        /// Determines the fill direction based on the specified "FillBehavior", "HorizontalDirection",
        /// and "VerticalDirection" values from the current XML element.
        /// </summary>
        /// <returns>
        /// A <see cref="FillDirection"/> value that represents the combined or individual direction
        /// based on the "FillBehavior" attribute. Returns the default <see cref="FillDirection"/> if
        /// no valid configuration is found.
        /// </returns>
        public FillDirection GetFillDirection()
        {
            var b = element.GetValue("FillBehavior")?.ToLower();
            var h = element.GetEnum<FillDirection>("HorizontalDirection");
            var v = element.GetEnum<FillDirection>("VerticalDirection");

            return b switch
            {
                "both" => v.Combine(h),
                "vertical" => v,
                "horizontal" => h,
                _ => throw new InvalidOperationException($"Could not determine fill behavior from value '{b}'")
            };
        }

        /// <summary>
        /// Adds a specified child XML element to the current XML element.
        /// </summary>
        /// <param name="child">
        /// The <see cref="XElement"/> to be added as a child of the current XML element.
        /// If the child is null, no action is performed.
        /// </param>
        /// <returns>
        /// The current <see cref="XElement"/> after the child element is added.
        /// </returns>
        public XElement AddChild(XElement? child)
        {
            if (child is not null)
                element.Add(child);

            return element;
        }

        /// <summary>
        /// Adds a new child XML element to the current element with the specified name and applies the provided configuration.
        /// </summary>
        /// <param name="name">The name of the child element to be created and added to the current element.</param>
        /// <param name="config">An action that takes the newly created <see cref="XElement"/> as a parameter
        /// and allows for configuration of the child element.</param>
        /// <returns>The current <see cref="XElement"/> with the newly added child element.</returns>
        public XElement AddChild(string name, Action<XElement> config)
        {
            var child = new XElement(name);
            config(child);
            element.Add(child);
            return element;
        }

        /// <summary>
        /// Adds a fill style to the current XML element based on the specified graphical brush and serialization function.
        /// </summary>
        /// <param name="brush">
        /// The <see cref="GraphicBrush"/> instance defining the style of the fill. If null, no fill style is added.
        /// </param>
        /// <param name="serializer">
        /// A function that serializes the provided <see cref="GraphicBrush"/> into an <see cref="XElement"/> for representation.
        /// </param>
        /// <returns>
        /// The original <see cref="XElement"/> with the fill style added, if applicable.
        /// </returns>
        public XElement AddFillStyle(GraphicBrush? brush, Func<GraphicBrush?, XElement?> serializer)
        {
            var type = brush?.GetType().Name.Replace("Brush", string.Empty) ?? "NoFill";
            var fill = new XElement("FillStyle", new XAttribute("FillType", type));

            var child = serializer(brush);
            if (child is not null)
                fill.Add(child);

            return element;
        }

        /// <summary>
        /// Adds an unfilled style to the current XML element based on the specified graphical brush and serializer.
        /// </summary>
        /// <param name="brush">
        /// The <see cref="GraphicBrush"/> representing the graphical brush to be serialized. If null, no unfilled style will be added.
        /// </param>
        /// <param name="serializer">
        /// A function that takes a <see cref="GraphicBrush"/> as input and returns an <see cref="XElement"/> representing the serialized brush.
        /// </param>
        /// <returns>
        /// The modified <see cref="XElement"/> with the unfilled style added, or the original element if the brush is null.
        /// </returns>
        public XElement AddUnfilledStyle(GraphicBrush? brush, Func<GraphicBrush?, XElement?> serializer)
        {
            var type = brush?.GetType().Name.Replace("Brush", string.Empty) ?? "NoFill";
            var fill = new XElement("UnfilledStyle", new XAttribute("FillType", type));

            var child = serializer(brush);
            if (child is not null)
                fill.Add(child);

            return element;
        }

        /// <summary>
        /// Adds a new child XML element representing a color to the current <see cref="XElement"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the new child XML element to be created.
        /// </param>
        /// <param name="color">
        /// The <see cref="GraphicColor"/> value to be represented by the new child element.
        /// The color's RGBA components are stored as attributes in the child element.
        /// </param>
        /// <returns>
        /// The updated <see cref="XElement"/> containing the newly added child element.
        /// </returns>
        public void AddColor(string name, GraphicColor color)
        {
            var child = new XElement(name,
                new XAttribute("R", color.R),
                new XAttribute("G", color.G),
                new XAttribute("B", color.B),
                new XAttribute("A", color.A)
            );

            element.Add(child);
        }

        /// <summary>
        /// Sets the value of a specified XML element or attribute based on the selector path.
        /// </summary>
        /// <param name="selector">
        /// A string specifying either an attribute (prefixed with '@') or a child element
        /// by its name, which will receive the specified value.
        /// </param>
        /// <param name="value">
        /// The value to assign to the specified XML element or attribute. If the value
        /// is null, the respective element or attribute will be removed.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/> that was updated with the specified value.
        /// </returns>
        public XElement SetValue(string selector, object? value)
        {
            var isAttribute = selector.StartsWith("@");
            var name = selector.TrimStart("@").ToString();

            if (isAttribute)
                element.SetAttributeValue(name, value);
            else
                element.SetElementValue(name, value);

            return element;
        }

        /// <summary>
        /// Updates the XML element's attributes to correspond to the specified graphical bounds.
        /// </summary>
        /// <param name="bounds">
        /// The <see cref="GraphicBounds"/> representing the left, top, width, and height values
        /// to assign to the corresponding attributes of the XML element.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/> updated with the new bounds attributes.
        /// </returns>
        public XElement SetBounds(GraphicBounds bounds)
        {
            element.SetAttributeValue("Left", bounds.Left);
            element.SetAttributeValue("Top", bounds.Top);
            element.SetAttributeValue("Width", bounds.Width);
            element.SetAttributeValue("Height", bounds.Height);
            return element;
        }

        /// <summary>
        /// Sets the origin of the graphical element by updating its "OriginX" and "OriginY" attributes.
        /// </summary>
        /// <param name="point">The <see cref="GraphicPoint" /> representing the new origin coordinates.</param>
        public XElement SetOrigin(GraphicPoint point)
        {
            element.SetAttributeValue("OriginX", point.X);
            element.SetAttributeValue("OriginY", point.Y);
            return element;
        }

        /// <summary>
        /// </summary>
        /// <param name="flip"></param>
        public void SetFlip(GraphicFlip? flip)
        {
            if (flip is null or GraphicFlip.None)
                return;

            element.SetAttributeValue("HasTransform", "True");

            switch (flip)
            {
                case GraphicFlip.Horizontal:
                    element.SetAttributeValue("Transform_0", "-1");
                    element.SetAttributeValue("Transform_3", "1");
                    break;
                case GraphicFlip.Vertical:
                    element.SetAttributeValue("Transform_0", "1");
                    element.SetAttributeValue("Transform_3", "-1");
                    break;
                case GraphicFlip.Both:
                    element.SetAttributeValue("Transform_0", "-1");
                    element.SetAttributeValue("Transform_3", "-1");
                    break;
                case GraphicFlip.None:
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(flip), flip, null);
            }
        }

        public XElement SetFillDirection(FillDirection direction)
        {
            /*var b = element.GetValue("FillBehavior")?.ToLower();
            var h = element.GetEnum<FillDirection>("HorizontalDirection");
            var v = element.GetEnum<FillDirection>("VerticalDirection");

            return b switch
            {
                "both" => v.Combine(h),
                "vertical" => v,
                "horizontal" => h,
                _ => throw new InvalidOperationException($"Could not determine fill behavior from value '{b}'")
            };*/
            return element;
        }

        /// <summary>
        /// Retrieves the value of the specified selector from an XML element.
        /// The selector can navigate through element names or attributes to extract the desired value.
        /// If the selector is null or empty, the value of the current element is returned.
        /// </summary>
        /// <param name="selector">
        /// A dot-separated path specifying the hierarchy of elements or an attribute name prefixed with '@'.
        /// If the selector is empty or null, the value of the current element is returned.
        /// </param>
        /// <returns>
        /// The value extracted from the specified selector as a string or null if the selector does
        /// not yield a valid element or attribute.
        /// </returns>
        private string? SelectValue(string? selector)
        {
            if (string.IsNullOrEmpty(selector))
                return element.Value;

            var current = element;
            var span = selector.AsSpan();

            while (!span.IsEmpty)
            {
                var dotIndex = span.IndexOf('.');
                var segment = dotIndex >= 0 ? span[..dotIndex] : span;
                span = dotIndex >= 0 ? span[(dotIndex + 1)..] : default;

                if (segment.IsEmpty)
                    continue;

                if (segment.StartsWith("@"))
                    return current?.Attribute(segment[1..].ToString())?.Value;

                current = current?.Element(segment.ToString());
            }

            return current?.Value;
        }

        /// <summary>
        /// Navigates the XML structure starting from the current element and selects a descendant element
        /// based on the dot-separated selector string.
        /// </summary>
        /// <param name="selector">
        /// A dot-separated string that represents the path of descendant elements to navigate.
        /// Each segment in the string corresponds to a child element. If the selector is null or empty, the current element is returned.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/> corresponding to the specified path, or null if no matching element is found.
        /// </returns>
        private XElement? SelectElement(string? selector)
        {
            if (string.IsNullOrEmpty(selector))
                return element;

            var current = element;
            var span = selector.AsSpan();

            while (!span.IsEmpty)
            {
                var dotIndex = span.IndexOf('.');
                var segment = dotIndex >= 0 ? span[..dotIndex] : span;
                span = dotIndex >= 0 ? span[(dotIndex + 1)..] : default;

                if (segment.IsEmpty)
                    continue;

                current = current?.Element(segment.ToString());
            }

            return current;
        }
    }

    /// <summary>
    /// Compares the specified string with another string for equality, using a case-insensitive comparison.
    /// </summary>
    /// <param name="text">
    /// The input string to compare. This can be null.
    /// </param>
    /// <param name="value">
    /// The string to compare against. This can also be null.
    /// </param>
    /// <returns>
    /// A boolean value indicating whether the two strings are equal, ignoring case.
    /// </returns>
    public static bool Is(this string? text, string? value)
    {
        return StringComparer.OrdinalIgnoreCase.Equals(text, value);
    }
}