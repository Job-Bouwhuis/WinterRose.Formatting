using WinterRose.Formatting.TimeFormats;

internal sealed record SwitchNode : DateFormatNode
{
    public string Variable { get; }

    public IReadOnlyDictionary<object, DateFormatNode> Cases { get; }

    public DateFormatNode Default { get; }

    public SwitchNode(
        string variable,
        IReadOnlyDictionary<object, DateFormatNode> cases,
        DateFormatNode @default)
    {
        Variable = variable;
        Cases = cases;
        Default = @default;
    }
}