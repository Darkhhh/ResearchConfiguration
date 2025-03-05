using System.Globalization;

namespace ResearchConfiguration.DataStructures.Primitives;

public struct FloatWrap(float value) : IXmlConvertible
{
    public float Value { get; set; } = value;
    
    public static object FromString(string input) => new FloatWrap(Convert.ToSingle(input));
    
    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public static implicit operator float(FloatWrap value) => value.Value;
}