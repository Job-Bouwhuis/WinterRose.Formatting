namespace WinterRose.Formatting.TimeFormats
{
    internal sealed record DurationCondition(
        DateComparison Comparison,
        TimeSpan Duration
    ) : DateCondition;
}