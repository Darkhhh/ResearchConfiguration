namespace ResearchConfiguration.DataStructures.Custom;

public struct EnumWrap<T>(T value) : IXmlConvertible where T : struct, Enum
{
    public T Value { get; set; } = value;

    public override string ToString() => Value.ToString();

    public static object FromString(string input) => new EnumWrap<T>(GetEnum<T>(input));
    
    private static TEnum GetEnum<TEnum>(string value) where TEnum : struct, Enum
    {
        var enumArray = Enum.GetNames(typeof(TEnum)).ToArray();
        var enumValues = Enum.GetValues(typeof(TEnum));
        if (enumValues is null) throw new Exception($"Can not cast to enum from {typeof(TEnum).FullName}");
        for (var i = 0; i < enumArray.Length; i++)
        {
            if (enumArray[i] == value)
            {
                var result = (TEnum)(enumValues.GetValue(i) ?? 
                                 throw new Exception($"Can not get value from {typeof(TEnum).FullName} enum"));
                return result;
            }
        }
        throw new Exception("Incorrect enum value");
    }
}