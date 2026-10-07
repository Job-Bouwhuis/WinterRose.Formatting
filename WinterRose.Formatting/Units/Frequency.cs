namespace WinterRose.Formatting.Units;

public readonly record struct Frequency(decimal Value, FrequencyUnit Unit)
{
    public Frequency To(FrequencyUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal hertz = Unit switch
        {
            FrequencyUnit.Hertz => Value,
            FrequencyUnit.Kilohertz => Value * 1000m,
            FrequencyUnit.Megahertz => Value * 1_000_000m,
            FrequencyUnit.Gigahertz => Value * 1_000_000_000m,
            FrequencyUnit.Terahertz => Value * 1_000_000_000_000m,
            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            FrequencyUnit.Hertz => hertz,
            FrequencyUnit.Kilohertz => hertz / 1000m,
            FrequencyUnit.Megahertz => hertz / 1_000_000m,
            FrequencyUnit.Gigahertz => hertz / 1_000_000_000m,
            FrequencyUnit.Terahertz => hertz / 1_000_000_000_000m,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        return new Frequency(NumberFormatter.RoundForDisplay(converted), unit);
    }
}