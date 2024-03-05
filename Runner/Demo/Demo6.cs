using ResearchConfiguration.Configuration;
using ResearchConfiguration.Extensions;

namespace Runner.Demo;

public static partial class Demo
{
    public static void Demo6()
    {
        var reader = Loader.Load("Test", "Configs", getFromProjectDirectory: true);

        var simpleString = reader.Get("MaxStateTime");

        var simpleInt = reader.GetAsInt("MaxStateTime");

        var simpleDouble = reader.GetAsDouble("Nodes", "ObserveRange");

        var simpleEnum = reader.GetAsEnum<AreaTypeEnum>("Area", "Type");
    }

    public enum AreaTypeEnum
    {
        Rectangle, Circle, Triangle
    }
}
