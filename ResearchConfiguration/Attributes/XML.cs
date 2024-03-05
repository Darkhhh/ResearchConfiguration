namespace ResearchConfiguration.Attributes;


[AttributeUsage(AttributeTargets.Property)]
public class NodeNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}


[AttributeUsage(AttributeTargets.Property)]
public class AttributeNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}


[AttributeUsage(AttributeTargets.Property)]
public class DoNotUseInDescription : Attribute;