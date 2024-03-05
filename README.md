# ResearchConfiguration

Проект представляет собой систему, позволяющую читать конфигурационные файлы в формате XML. Появился ввиду необходимости упрощения и унифицирования работы с входными параметрами исследования, которые могут часто меняться и иметь различные строковые представления.

## ResearchReader

Класс, читающий данные из XML файла. Поддерживает чтение значения элемента и его атрибутов. 
> [!CAUTION]
> Не поддерживает объявление нескольких нод с одним именем. Читаться будет только первая встреченная.

Возвращает значение как строку, однако через методы расширения `ReaderExtensions.cs` доступны приведения к некоторым базовым типам:

```csharp
var reader = Loader.Load("Test", "Configs", getFromProjectDirectory: true);

var simpleString = reader.Get("MaxStateTime");

var simpleInt = reader.GetAsInt("MaxStateTime");

var simpleDouble = reader.GetAsDouble("Nodes", "ObserveRange");

var simpleEnum = reader.GetAsEnum<AreaTypeEnum>("Area", "Type");


public enum AreaTypeEnum
{
    Rectangle, Circle, Triangle
}
```

> [!TIP]
> Статический класс `Loader` упрощает создание ридера на основе заданного файла или создание нескольких ридеров из файлов одной папки.

Помимо этого можно читать массивы, проверять на наличие атрибута или ноды, а также сравнивать значения:

```csharp
var doubleArray = reader.GetDoubles("SomeNode", separator:";");

if (reader.HasAttribute("SearchNode", "ThisAttribute))
{
  //Do something...
}

if (!reader.IsValueEqual("SaveResults", "Handles", "OnFinish") return;
```

## Injecting

Базового взаимодействия с ридером может быть достаточно, если требуемых значений немного, в противном случае удобнее пользоваться инъекцией.

Для инъекции значений достаточно определить свой класс конфигурации, в котором объявить публичные свойства, помеченные специальными атрибутами:
```csharp
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
```

После этого нужно создать экземпляр класса `Injector` и воспользоваться статическим классом `RConfiguration`:
```csharp
var reader = Loader.Load("Test", "Configs", getFromProjectDirectory:true);

var config = new DemoConfig1();
RConfiguration.Fill(config, new Injector(reader));
```

Для примитивных типов используются специальные обертки в виде структур. Однако можно легко определять собственные типы и как их нужно интерпретировать. Для этого необходимо наследоваться от интерфейса `IXmlConvertible`:
```csharp
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
```

Теперь, достаточно дать знать инъектору об этом типе, перед внедрением значений, и пользоваться заданной оберткой:
```csharp
var reader = Loader.Load("Test", "Configs");
var injector = new Injector(reader);
injector.AddCustomType<IntArray>();

RConfiguration.Fill(config, injector);
```

> [!TIP]
> Заполненные конфигурации затем можно легко клонировать с помощью `RConfiguration.Clone`.

## Output

Одной важной вещью в исследовании является сопоставление конфигурации и полученных результатов, для этого доступны два метода: `RConfiguration.ToDescription` и `RConfiguration.ToFileName`. Первый метод позволяет в одну строчку вывести все наименования свойств и их значения, второй же нужен для более компактного вывода лишь самых важных параметров конфигурации.

По умолчанию в описание включаются все свойства, однако по желанию некоторые можно убрать с помощью атрибута `DoNotUseInDescription`:
```csharp
[NodeName("ObservableObjects")][AttributeName("InitiallyActive")][DoNotUseInDescription]
public BoolWrap InitiallyActive { get; set; }
```

Для формирования компактного описания, включающего лишь некоторые свойства нужно воспользоваться атрибутом `Title`:
```csharp
[NodeName("Area")][AttributeName("Size")][Title(order:0, shorter:"S")]
public LongWrap AreaSize { get; set; }
```

Значение `order` объявляет порядок, от меньшего к большему. Значение `shorter` позволяет использовать более компактное наименование для свойства, если же оно будет незаполнено, то будет использоваться полное название самого свойства.
