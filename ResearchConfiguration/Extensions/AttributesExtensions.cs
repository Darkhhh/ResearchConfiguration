using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ResearchConfiguration.Extensions;

public static class AttributesExtensions
{
    public static bool HasAttribute<T>(this MemberInfo member, [MaybeNullWhen(false)]out T value) where T : Attribute
    {
        value = null;
        foreach (var customAttribute in member.GetCustomAttributes())
        {
            if (customAttribute.GetType().IsAssignableTo(typeof(T)))
            {
                value = (T) customAttribute;
                return true;
            }
        }
        return false;
    }
}