namespace WinterRose.Formatting.Units;

public readonly record struct Distance(decimal Value, DistanceUnit Unit)
{
    public Distance To(DistanceUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal meters = Unit switch
        {
            DistanceUnit.Millimeter => Value / 1000m,
            DistanceUnit.Centimeter => Value / 100m,
            DistanceUnit.Meter => Value,
            DistanceUnit.Kilometer => Value * 1000m,

            DistanceUnit.Inch => Value * 0.0254m,
            DistanceUnit.Foot => Value * 0.3048m,
            DistanceUnit.Yard => Value * 0.9144m,
            DistanceUnit.Mile => Value * 1609.344m,
            DistanceUnit.NauticalMile => Value * 1852m,

            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            DistanceUnit.Millimeter => meters * 1000m,
            DistanceUnit.Centimeter => meters * 100m,
            DistanceUnit.Meter => meters,
            DistanceUnit.Kilometer => meters / 1000m,

            DistanceUnit.Inch => meters / 0.0254m,
            DistanceUnit.Foot => meters / 0.3048m,
            DistanceUnit.Yard => meters / 0.9144m,
            DistanceUnit.Mile => meters / 1609.344m,
            DistanceUnit.NauticalMile => meters / 1852m,

            _ => throw new ArgumentOutOfRangeException()
        };

        return new Distance(NumberFormatter.RoundForDisplay(converted), unit);
    }
}
