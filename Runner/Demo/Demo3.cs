using ResearchConfiguration;
using ResearchConfiguration.Attributes;
using ResearchConfiguration.Configuration;
using ResearchConfiguration.DataStructures.Primitives;

namespace Runner.Demo;

public static partial class Demo
{
    public static void Demo3()
    {
        var reader = Loader.Load("Test", "Configs", getFromProjectDirectory:true);

        var config = new DemoConfig3();
        RConfiguration.Fill(config, new Injector(reader));
        
        Console.WriteLine($"Title{RConfiguration.ToFileName(config)}");
    }
}

public class DemoConfig3
{
    [NodeName("Environment")][AttributeName("Experiments")][DoNotUseInDescription][Title(order:0, shorter:"Exps")]
    public IntWrap Experiments { get; set; }
    
    [NodeName("ObservableObjects")][AttributeName("InitiallyActive")][DoNotUseInDescription]
    public BoolWrap InitiallyActive { get; set; }
    
    [NodeName("Area")][AttributeName("Size")]
    public LongWrap AreaSize { get; set; }
    
    [NodeName("Nodes")][AttributeName("ObserveRange")][Title(order:0, shorter:"RO")]
    public FloatWrap ObserveRange { get; set; }
}