namespace ResearchConfiguration.DataStructures.Primitives;

public struct LongWrap(long value) : IXmlConvertible
{
    public long Value { get; set; } = value;
    
    public static object FromString(string input) => new LongWrap(Convert.ToInt64(input));
    
    public override string ToString() => Value.ToString();
}