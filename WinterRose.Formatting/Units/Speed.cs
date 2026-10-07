namespace WinterRose.Formatting.Units;

public readonly record struct Speed(decimal Value, SpeedUnit Unit)
{
    public Speed To(SpeedUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal metersPerSecond = Unit switch
        {
            SpeedUnit.MetersPerSecond => Value,
            SpeedUnit.KilometersPerHour => Value / 3.6m,
            SpeedUnit.FeetPerSecond => Value * 0.3048m,
            SpeedUnit.MilesPerHour => Value * 1609.344m / 3600m,
            SpeedUnit.Knots => Value * 1852m / 3600m,

            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            SpeedUnit.MetersPerSecond => metersPerSecond,
            SpeedUnit.KilometersPerHour => metersPerSecond * 3.6m,
            SpeedUnit.FeetPerSecond => metersPerSecond / 0.3048m,
            SpeedUnit.MilesPerHour => metersPerSecond * 3600m / 1609.344m,
            SpeedUnit.Knots => metersPerSecond * 3600m / 1852m,

            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        return new Speed(NumberFormatter.RoundForDisplay(converted), unit);
    }
}