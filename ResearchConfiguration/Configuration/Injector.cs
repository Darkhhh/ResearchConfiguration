using System.Reflection;
using ResearchConfiguration.Attributes;
using ResearchConfiguration.DataStructures;
using ResearchConfiguration.DataStructures.Primitives;
using ResearchConfiguration.Extensions;

namespace ResearchConfiguration.Configuration;

public delegate object FromString(string input);

public class Injector(Reader reader)
{
    private readonly List<(Type Type, FromString InstanceGenerator)> _injectedTypes = DefaultTypes();

    public Injector AddCustomType<T>() where T : IXmlConvertible
    {
        _injectedTypes.Add((typeof(T), T.FromString));
        return this;
    }
    
    
    internal void InjectValuesTo<T>(T obj) where T : class
    {
        foreach (var propertyInfo in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance))
        {
            if (!propertyInfo.PropertyType.IsAssignableTo(typeof(IXmlConvertible))) continue;
            if (!propertyInfo.HasAttribute<NodeNameAttribute>(out var nodeName)) continue;

            var value = propertyInfo.HasAttribute<AttributeNameAttribute>(out var attrName) ? 
                reader.Get(nodeName.Name, attrName.Name) : reader.Get(nodeName.Name);

            foreach (var type in _injectedTypes.Where(type => propertyInfo.PropertyType.IsAssignableTo(type.Type)))
            {
                propertyInfo.SetValue(obj, type.InstanceGenerator.Invoke(value));
                break;
            }
        }
    }


    private static List<(Type Type, FromString InstanceGenerator)> DefaultTypes()
    {
        var list = new List<(Type Type, FromString InstanceGenerator)>
        {
            (typeof(IntWrap), IntWrap.FromString), 
            (typeof(DoubleWrap), DoubleWrap.FromString), 
            (typeof(FloatWrap), FloatWrap.FromString), 
            (typeof(BoolWrap), BoolWrap.FromString), 
            (typeof(LongWrap), LongWrap.FromString),
        };

        return list;
    }
}