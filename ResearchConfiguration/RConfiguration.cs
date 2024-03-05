using System.Reflection;
using System.Text;
using ResearchConfiguration.Attributes;
using ResearchConfiguration.Configuration;
using ResearchConfiguration.Extensions;

namespace ResearchConfiguration;

public static class RConfiguration
{
    public static void Fill<T>(T config, Injector injector) where T : class
    {
        injector.InjectValuesTo(config);
    }
    
    public static string ToDescription<T>(T config)
    {
        var builder = new StringBuilder();
        builder.Append("Parameters: ");
        foreach (var propertyInfo in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                         BindingFlags.Instance))
        {
            if (propertyInfo.HasAttribute<DoNotUseInDescription>(out _)) continue;

            var title = propertyInfo.Name;
            var value = propertyInfo.GetValue(config);
            var str = value is null ? string.Empty : value.ToString();
            builder.Append($"\n{title} : {str};");
        }
        
        return builder.ToString();
    }

    public static string ToFileName<T>(T config)
    {
        var q = new PriorityQueue<string, int>();
        var builder = new StringBuilder();
        foreach (var propertyInfo in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance))
        {
            if (!propertyInfo.HasAttribute<TitleAttribute>(out var titleData)) continue;
            
            var title = titleData.HasShortTitle ? titleData.ShortTitle : propertyInfo.Name;
            
            var value = propertyInfo.GetValue(config);
            var str = value is null ? string.Empty : value.ToString();
            q.Enqueue($" {title}={str}", titleData.Order);
        }
        while (q.TryDequeue(out var data, out _))
        {
            builder.Append(data);
        }
        return builder.ToString();
    }

    public static T Clone<T>(T config) where T : new()
    {
        var result = new T();
        foreach (var propertyInfo in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance))
        {
            propertyInfo.SetValue(result, propertyInfo.GetValue(config));
        }

        return result;
    }
}