namespace ResearchConfiguration.DataStructures.Primitives;

public struct BoolWrap(bool value) : IXmlConvertible
{
    public bool Value { get; set; } = value;

    public static object FromString(string input) => new BoolWrap(Convert.ToBoolean(input));

    public override string ToString() => Value.ToString();

    public static implicit operator bool(BoolWrap value) => value.Value;
}