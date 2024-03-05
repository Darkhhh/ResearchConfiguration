using ResearchConfiguration;
using ResearchConfiguration.Attributes;
using ResearchConfiguration.Configuration;
using ResearchConfiguration.DataStructures.Primitives;

namespace Runner.Demo;

public static partial class Demo
{
    public static void Demo1()
    {
        var reader = Loader.Load("Test", "Configs", getFromProjectDirectory:true);

        var config = new DemoConfig1();
        RConfiguration.Fill(config, new Injector(reader));
        
        Console.WriteLine(RConfiguration.ToDescription(config));
    }
}

public class DemoConfig1
{
    [NodeName("Environment")][AttributeName("Experiments")]
    public IntWrap Experiments { get; set; }
    
    [NodeName("ObservableObjects")][AttributeName("InitiallyActive")]
    public BoolWrap InitiallyActive { get; set; }
    
    [NodeName("Area")][AttributeName("Size")]
    public LongWrap AreaSize { get; set; }
    
    [NodeName("Nodes")][AttributeName("ObserveRange")]
    public FloatWrap ObserveRange { get; set; }
}