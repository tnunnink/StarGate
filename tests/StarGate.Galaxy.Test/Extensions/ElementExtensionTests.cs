using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;
using StarGate.Galaxy;

namespace StarGate.Galaxy.Tests;

[TestFixture]
public class ElementExtensionTests
{
    private const string FillStyleData =
        """
        <FillStyleData VerticalPercentFill="100" HorizontalPercentFill="100">
            <FillStyle FillType="Pattern">
                <PatternFill>
                    <Transparency>0</Transparency>
                    <BackColor R="255" G="0" B="255" A="255"/>
                    <ForeColor R="85" G="125" B="170" A="255"/>
                    <HatchStyle>Percent70</HatchStyle>
                </PatternFill>
            </FillStyle>
            <UnfilledStyle FillType="Solid">
                <SolidFill>
                    <FillColor R="123" G="234" B="213" A="255"/>
                    <Transparency>0</Transparency>
                </SolidFill>
            </UnfilledStyle>
            <FillBehavior>Both</FillBehavior>
            <HorizontalDirection>Right</HorizontalDirection>
            <VerticalDirection>Up</VerticalDirection>
            <FillOrientation>RelativeToGraphic</FillOrientation>
        </FillStyleData>
        """;

    [Test]
    public void GetStruct_DirectAttributeSelector_ShouldReturnParsedValue()
    {
        var element = XElement.Parse(FillStyleData);

        var result = element.GetStruct("@VerticalPercentFill", double.Parse);

        result.Should().Be(100.0);
    }

    [Test]
    public void GetEnum_NestedElementTextValue_ShouldReturnParsedValue()
    {
        var element = XElement.Parse(FillStyleData);

        var result = element.GetEnum<FillOrientation>("FillOrientation");

        result.Should().Be(FillOrientation.RelativeToGraphic);
    }
}