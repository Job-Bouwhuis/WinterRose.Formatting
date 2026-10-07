namespace WinterRose.Formatting.Units;

public readonly record struct Temperature(decimal Value, TemperatureScale Scale)
{
    public Temperature To(TemperatureScale scale)
    {
        if (Scale == scale)
            return this;

        decimal kelvin = Scale switch
        {
            TemperatureScale.Celsius => Value + 273.15m,
            TemperatureScale.Fahrenheit => (Value + 459.67m) * 5m / 9m,
            TemperatureScale.Kelvin => Value,
            TemperatureScale.Rankine => Value * 5m / 9m,
            TemperatureScale.Newton => Value * 100m / 33m + 273.15m,
            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = scale switch
        {
            TemperatureScale.Celsius => kelvin - 273.15m,
            TemperatureScale.Fahrenheit => kelvin * 9m / 5m - 459.67m,
            TemperatureScale.Kelvin => kelvin,
            TemperatureScale.Rankine => kelvin * 9m / 5m,
            TemperatureScale.Newton => (kelvin - 273.15m) * 33m / 100m,
            _ => throw new ArgumentOutOfRangeException()
        };

        return new Temperature(NumberFormatter.RoundForDisplay(converted), scale);
    }

    public override string ToString() => $"{Value} {GetSymbol(Scale)}";

    private static string GetSymbol(TemperatureScale scale) => scale switch
    {
        TemperatureScale.Celsius => "°C",
        TemperatureScale.Fahrenheit => "°F",
        TemperatureScale.Kelvin => "K",
        TemperatureScale.Rankine => "°R",
        TemperatureScale.Newton => "°N",
        _ => throw new ArgumentOutOfRangeException(nameof(scale))
    };
}