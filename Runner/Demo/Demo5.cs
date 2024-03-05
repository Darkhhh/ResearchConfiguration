using ResearchConfiguration;
using ResearchConfiguration.Configuration;

namespace Runner.Demo;

public static partial class Demo
{
    public static void Demo5()
    {
        var reader = Loader.Load("Test", "Configs", getFromProjectDirectory:true);

        var config = new DemoConfig1();
        RConfiguration.Fill(config, new Injector(reader));
        
        Console.WriteLine(RConfiguration.ToDescription(config));

        var clone = RConfiguration.Clone(config);
        
        Console.WriteLine(RConfiguration.ToDescription(clone));
    }
}