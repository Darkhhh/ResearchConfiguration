using System.Globalization;

namespace ResearchConfiguration.DataStructures.Primitives;

public struct DoubleWrap(double value) : IXmlConvertible
{
    public double Value { get; set; } = value;

    public static object FromString(string input) => new DoubleWrap(Convert.ToDouble(input));
    
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}