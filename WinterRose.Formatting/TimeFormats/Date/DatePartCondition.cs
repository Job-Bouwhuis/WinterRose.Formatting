using WinterRose.Formatting.TimeFormats;

internal sealed record DatePartCondition(string Part, DateComparison Comparison, object Value) : DateCondition;
