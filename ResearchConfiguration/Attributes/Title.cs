namespace ResearchConfiguration.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TitleAttribute : Attribute, IComparable<TitleAttribute>
{
    public int Order { get; }
    public bool HasShortTitle { get; }
    public string ShortTitle { get; }
    
    public TitleAttribute(int order = 0, string? shorter = null)
    {
        Order = order;
        HasShortTitle = shorter is not null;
        ShortTitle = (HasShortTitle ? shorter : string.Empty)!;
    }

    public int CompareTo(TitleAttribute? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Order.CompareTo(other.Order);
    }
}