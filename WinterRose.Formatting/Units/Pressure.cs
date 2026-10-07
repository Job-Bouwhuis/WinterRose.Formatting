namespace WinterRose.Formatting.Units;

public readonly record struct Pressure(decimal Value, PressureUnit Unit)
{
    public Pressure To(PressureUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal pascals = Unit switch
        {
            PressureUnit.Pascal => Value,
            PressureUnit.Kilopascal => Value * 1000m,
            PressureUnit.Megapascal => Value * 1_000_000m,
            PressureUnit.Bar => Value * 100_000m,
            PressureUnit.Psi => Value * 6894.757293168m,
            PressureUnit.StandardAtmosphere => Value * 101325m,
            PressureUnit.MillimeterOfMercury => Value * 133.322387415m,
            PressureUnit.InchOfMercury => Value * 3386.389m,
            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            PressureUnit.Pascal => pascals,
            PressureUnit.Kilopascal => pascals / 1000m,
            PressureUnit.Megapascal => pascals / 1_000_000m,
            PressureUnit.Bar => pascals / 100_000m,
            PressureUnit.Psi => pascals / 6894.757293168m,
            PressureUnit.StandardAtmosphere => pascals / 101325m,
            PressureUnit.MillimeterOfMercury => pascals / 133.322387415m,
            PressureUnit.InchOfMercury => pascals / 3386.389m,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        return new Pressure(NumberFormatter.RoundForDisplay(converted), unit);
    }
}
