namespace WinterRose.Formatting.Units;

public readonly record struct Force(decimal Value, ForceUnit Unit)
{
    public Force To(ForceUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal newtons = Unit switch
        {
            ForceUnit.Newton => Value,
            ForceUnit.Kilonewton => Value * 1000m,
            ForceUnit.Meganewton => Value * 1_000_000m,
            ForceUnit.PoundForce => Value * 4.4482216152605m,
            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            ForceUnit.Newton => newtons,
            ForceUnit.Kilonewton => newtons / 1000m,
            ForceUnit.Meganewton => newtons / 1_000_000m,
            ForceUnit.PoundForce => newtons / 4.4482216152605m,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        return new Force(NumberFormatter.RoundForDisplay(converted), unit);
    }
}
