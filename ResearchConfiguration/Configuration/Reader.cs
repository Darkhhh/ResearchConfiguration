using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace ResearchConfiguration.Configuration;

/// <summary>
/// Reads xml file as configuration file. Does not support several nodes with same name (handles only first).
/// </summary>
public class Reader
{
    #region Private Values

    private readonly XmlElement _root;
    private readonly Dictionary<string, XmlElement>? _cachedElements;

    #endregion
    

    #region Properties

    /// <summary>
    /// Returns file name like "config.xml"
    /// </summary>
    public string FileNameWithExtension { get; init; }
    /// <summary>
    /// Returns file name like "config" without ".xml"
    /// </summary>
    public string FileName => FileNameWithExtension.Replace(".xml", string.Empty);

    #endregion
    
    
    public Reader(string path)
    {
        FileNameWithExtension = Path.GetFileName(path);
        using var reader = new XmlTextReader(path);
        reader.WhitespaceHandling = WhitespaceHandling.None;
        //reader.MoveToContent();
        reader.Read();
        var xmlDocument = new XmlDocument();
        xmlDocument.Load(reader);
        //xmlDocument.LoadXml(_filename);
        var xRoot = xmlDocument.DocumentElement;

        _root = xRoot ?? throw new Exception("Incorrect file structure");
        _cachedElements = new Dictionary<string, XmlElement>();
    }


    #region Get

    public string Get(XmlElement root, string nodeName)
    {
        if (!Find(root, nodeName, out var xmlElement))
            throw new Exception($"Can't find element with name {nodeName}");
        if (xmlElement is null) throw new NullReferenceException();

        var value = xmlElement.InnerText ??
                    throw new NullReferenceException($"XmlElement {nodeName} does not contain any value");

        return value;
    }

    public string Get(string nodeName) => Get(_root, nodeName);

    public string Get(XmlElement root, string nodeName, string attributeName)
    {
        if (!Find(root, nodeName, out var xmlElement))
            throw new Exception($"Can't find element with name {nodeName}");
        if (xmlElement is null) throw new NullReferenceException();
        if (!xmlElement.HasAttributes || xmlElement.Attributes is null) 
            throw new ArgumentException($"Element {nodeName} does not have attributes");
        var attr = xmlElement.Attributes.GetNamedItem(attributeName) ??
                   throw new NullReferenceException();
        return attr.Value ?? throw new NullReferenceException();
    }
    public string Get(string nodeName, string attributeName) => Get(_root, nodeName, attributeName);

    #endregion


    #region Try Get

    public bool TryGet(XmlElement root, string name, [MaybeNullWhen(false)] out string value)
    {
        value = null;
        if (!Find(root, name, out var xmlElement)) return false;
        if (xmlElement is null) throw new NullReferenceException();

        value = xmlElement.InnerText ??
                throw new NullReferenceException($"XmlElement {name} does not contain any value");
        
        return true;
    }
    public bool TryGet(string name, [MaybeNullWhen(false)] out string value) => TryGet(_root, name, out value);

    public bool TryGet(XmlElement root, string name, string attributeName, out string value)
    {
        value = string.Empty;
        if (!Find(root, name, out var xmlElement)) return false;
        if (xmlElement is null) throw new NullReferenceException();
        if (!xmlElement.HasAttributes) return false;
        var attr = xmlElement.Attributes.GetNamedItem(attributeName) ??
                   throw new NullReferenceException();
        if (attr.Value != null) value = attr.Value;
        else return false;
        return true;
    }

    public bool TryGet(string name, string attributeName, out string value) => TryGet(_root, name, attributeName, out value);

    #endregion


    #region XmlElement

    public bool HasNode(string name) => Find(_root, name, out _);

    public XmlElement GetNode(string name)
    {
        Find(_root, name, out var e);
        return e ?? throw new NullReferenceException($"Can not find element {name}");
    }

    #endregion
    
    private bool Find(XmlElement root, string name, [MaybeNullWhen(false)] out XmlElement element)
    {
        if (_cachedElements is null) throw new NullReferenceException();
        if (_cachedElements.TryGetValue(name, out var e))
        {
            element = e;
            return true;
        }

        foreach (var x in root)
        {
            if (x is null || x is XmlComment || x is not XmlElement childNode) continue;

            if (childNode.Name != name) continue;
            
            element = childNode;
            if (_cachedElements is null) throw new NullReferenceException();
            _cachedElements.Add(name, childNode);
            return true;
        }

        foreach (var x in root)
        {
            if (x is null || x is XmlComment || x is not XmlElement childNode) continue;
            if (!childNode.HasChildNodes) continue;
            if (Find(childNode, name, out element)) return true;
        }

        element = null;
        return false;
    }
}