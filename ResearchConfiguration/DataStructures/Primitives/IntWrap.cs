namespace ResearchConfiguration.DataStructures.Primitives;

public struct IntWrap(int value) : IXmlConvertible
{
    public int Value { get; set; } = value;
    
    public static object FromString(string input) => new IntWrap(Convert.ToInt32(input));
    
    public override string ToString() => Value.ToString();
}