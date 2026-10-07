namespace WinterRose.Formatting.Units;

public readonly record struct Power(decimal Value, PowerUnit Unit)
{
    public Power To(PowerUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal watts = Unit switch
        {
            PowerUnit.Watt => Value,
            PowerUnit.Kilowatt => Value * 1000m,
            PowerUnit.Megawatt => Value * 1_000_000m,
            PowerUnit.Gigawatt => Value * 1_000_000_000m,
            PowerUnit.Horsepower => Value * 745.69987158227022m,
            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            PowerUnit.Watt => watts,
            PowerUnit.Kilowatt => watts / 1000m,
            PowerUnit.Megawatt => watts / 1_000_000m,
            PowerUnit.Gigawatt => watts / 1_000_000_000m,
            PowerUnit.Horsepower => watts / 745.69987158227022m,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        return new Power(NumberFormatter.RoundForDisplay(converted), unit);
    }
}
