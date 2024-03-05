using System.Text;
using ResearchConfiguration;
using ResearchConfiguration.Attributes;
using ResearchConfiguration.Configuration;
using ResearchConfiguration.DataStructures;
using ResearchConfiguration.DataStructures.Primitives;

namespace Runner.Demo;

public static partial class Demo
{
    public static void Demo4()
    {
        var config = new DemoConfig4();

        var reader = Loader.Load("Test", "Configs");
        var injector = new Injector(reader);
        injector.AddCustomType<IntArray>();

        RConfiguration.Fill(config, injector);

        Console.WriteLine(RConfiguration.ToDescription(config));

        Console.WriteLine($"Title: {RConfiguration.ToFileName(config)}");
    }
}

public class DemoConfig4
{
    [NodeName("Environment")][AttributeName("Experiments")][Title(order:2, shorter:"Exp")]
    public IntWrap Experiments { get; set; }
    
    [NodeName("ObservableObjects")][AttributeName("InitiallyActive")][DoNotUseInDescription]
    public BoolWrap InitiallyActive { get; set; }
    
    [NodeName("Area")][AttributeName("Size")][Title(order:0, shorter:"S")]
    public LongWrap AreaSize { get; set; }
    
    [NodeName("Nodes")][AttributeName("ObserveRange")]
    public FloatWrap ObserveRange { get; set; }

    [NodeName("ObservableObjects")] [AttributeName("Number")]
    public IntArray Data { get; set; } = null!;
}

public class IntArray(int[] value) : IXmlConvertible
{
    public int[] Value { get; set; } = value;
    
    public static object FromString(string input)
    {
        var arrayStr = input.Split(";");
        var result = new int[arrayStr.Length];
        for (var i = 0; i < arrayStr.Length; i++)
        {
            result[i] = Convert.ToInt32(arrayStr[i].Trim());
        }
        return new IntArray(result);
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var i in Value) builder.Append($"{i} ");
        return builder.ToString();
    }
}