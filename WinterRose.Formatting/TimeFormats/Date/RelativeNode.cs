namespace WinterRose.Formatting.TimeFormats
{
    internal sealed record RelativeNode(
        bool Short,
        bool Calendar
    ) : DateFormatNode;
}