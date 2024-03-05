namespace ResearchConfiguration.DataStructures;

public interface IXmlConvertible
{
    public static abstract object FromString(string input);
}