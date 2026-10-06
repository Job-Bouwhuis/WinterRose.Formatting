namespace WinterRose.Formatting.TimeFormats;

internal sealed record ConditionalNode(
    DateCondition Condition,
    DateFormatNode WhenTrue,
    DateFormatNode WhenFalse
) :DateFormatNode;
