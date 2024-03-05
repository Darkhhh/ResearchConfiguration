using ResearchConfiguration.Configuration;

namespace ResearchConfiguration.Extensions;

public static class ReaderExtensions
{
    public static bool IsValueEqual(this Reader reader, string expectedValue, string nodeName, string attributeName)
    {
        if (!reader.TryGet(nodeName, attributeName, out var actualValue)) return false;
        return actualValue == expectedValue;
    }
    public static bool IsValueEqual(this Reader reader, string expectedValue, string nodeName)
    {
        if (!reader.TryGet(nodeName, out var actualValue)) return false;
        return actualValue == expectedValue;
    }
    
    
    public static int GetAsInt(this Reader reader, string nodeName, string attributeName)
    {
        var str = reader.Get(nodeName, attributeName);
        return Convert.ToInt32(str.Trim());
    }
    public static int GetAsInt(this Reader reader, string nodeName)
    {
        var str = reader.Get(nodeName);
        return Convert.ToInt32(str.Trim());
    }

    
    public static bool GetAsBool(this Reader reader, string nodeName, string attributeName)
    {
        var str = reader.Get(nodeName, attributeName);
        return Convert.ToBoolean(str.Trim());
    }
    public static bool GetAsBool(this Reader reader, string nodeName)
    {
        var str = reader.Get(nodeName);
        return Convert.ToBoolean(str.Trim());
    }
    
    
    public static double GetAsDouble(this Reader reader, string nodeName, string attributeName)
    {
        var str = reader.Get(nodeName, attributeName); 
        return Convert.ToDouble(str.Trim());
    }
    public static double GetAsDouble(this Reader reader, string nodeName)
    {
        var str = reader.Get(nodeName); 
        return Convert.ToDouble(str.Trim());
    }


    public static float GetAsFloat(this Reader reader, string nodeName, string attributeName)
    {
        var str = reader.Get(nodeName, attributeName); 
        return Convert.ToSingle(str.Trim());
    }
    public static float GetAsFloat(this Reader reader, string nodeName)
    {
        var str = reader.Get(nodeName); 
        return Convert.ToSingle(str.Trim());
    }
    
    
    public static T GetAsEnum<T>(this Reader reader, string nodeName, string attributeName) where T : struct, Enum
    {
        var str = reader.Get(nodeName, attributeName);
        return GetEnum<T>(str.Trim());
    }
    public static T GetAsEnum<T>(this Reader reader, string nodeName) where T : struct, Enum
    {
        var str = reader.Get(nodeName);
        return GetEnum<T>(str.Trim());
    }
    private static T GetEnum<T>(string value) where T : struct, Enum
    {
        var enumArray = Enum.GetNames(typeof(T)).ToArray();
        var enumValues = Enum.GetValues(typeof(T));
        if (enumValues is null) throw new Exception($"Can not cast to enum from {typeof(T).FullName}");
        for (var i = 0; i < enumArray.Length; i++)
        {
            if (enumArray[i] == value)
            {
                var result = (T)(enumValues.GetValue(i) ?? 
                                 throw new Exception($"Can not get value from {typeof(T).FullName} enum"));
                return result;
            }
        }
        throw new Exception("Incorrect enum value");
    }
    
    
    public static bool HasAttribute(this Reader reader, string nodeName, string attributeName)
    {
        if (!reader.HasNode(nodeName)) return false;

        var node = reader.GetNode(nodeName);
        return node.HasAttribute(attributeName);
    }
    public static bool HasAttribute(this Reader reader, string nodeName, string attributeName, out string value)
    {
        value = string.Empty;
        if (!reader.HasNode(nodeName)) return false;

        var node = reader.GetNode(nodeName);
        var attr = node.Attributes.GetNamedItem(attributeName);
        var str = attr?.Value;
        if (str is null) return false;
        
        value = str.Trim();
        return true;
    }
    
    public static double[] GetDoubles(this Reader reader, string nodeName, string separator = "; ")
    {
        if (!reader.TryGet(nodeName, out var str)) return Array.Empty<double>();
        var arrayStr = str.Split(separator);
        var result = new double[arrayStr.Length];
        for (var i = 0; i < arrayStr.Length; i++)
        {
            result[i] = Convert.ToDouble(arrayStr[i].Trim());
        }
        return result;
    }
    public static float[] GetFloats(this Reader reader, string nodeName, string separator = "; ")
    {
        if (!reader.TryGet(nodeName, out var str)) return Array.Empty<float>();
        var arrayStr = str.Split(separator);
        var result = new float[arrayStr.Length];
        for (var i = 0; i < arrayStr.Length; i++)
        {
            result[i] = Convert.ToSingle(arrayStr[i].Trim());
        }
        return result;
    }
    public static int[] GetIntegers(this Reader reader, string nodeName, string separator = "; ")
    {
        if (!reader.TryGet(nodeName, out var str)) return Array.Empty<int>();
        var arrayStr = str.Split(separator);
        var result = new int[arrayStr.Length];
        for (var i = 0; i < arrayStr.Length; i++)
        {
            result[i] = Convert.ToInt32(arrayStr[i].Trim());
        }
        return result;
    }
    public static bool[] GetBooleans(this Reader reader, string nodeName, string separator = "; ")
    {
        if (!reader.TryGet(nodeName, out var str)) return Array.Empty<bool>();
        var arrayStr = str.Split(separator);
        var result = new bool[arrayStr.Length];
        for (var i = 0; i < arrayStr.Length; i++)
        {
            result[i] = Convert.ToBoolean(arrayStr[i].Trim());
        }
        return result;
    }
}