namespace WinterRose.Formatting.TimeFormats;

internal sealed record SequenceNode(IReadOnlyList<DateFormatNode> Nodes) :DateFormatNode;