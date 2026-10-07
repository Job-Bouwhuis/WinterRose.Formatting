namespace WinterRose.Formatting.Units;

public readonly record struct Weight(decimal Value, WeightUnit Unit)
{
    public Weight To(WeightUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal grams = Unit switch
        {
            WeightUnit.Milligram => Value / 1000m,
            WeightUnit.Gram => Value,
            WeightUnit.Kilogram => Value * 1000m,
            WeightUnit.MetricTon => Value * 1_000_000m,

            WeightUnit.Ounce => Value * 28.349523125m,
            WeightUnit.Pound => Value * 453.59237m,
            WeightUnit.Stone => Value * 6350.29318m,
            WeightUnit.ShortTon => Value * 907_184.74m,
            WeightUnit.LongTon => Value * 1_016_046.9088m,

            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            WeightUnit.Milligram => grams * 1000m,
            WeightUnit.Gram => grams,
            WeightUnit.Kilogram => grams / 1000m,
            WeightUnit.MetricTon => grams / 1_000_000m,

            WeightUnit.Ounce => grams / 28.349523125m,
            WeightUnit.Pound => grams / 453.59237m,
            WeightUnit.Stone => grams / 6350.29318m,
            WeightUnit.ShortTon => grams / 907_184.74m,
            WeightUnit.LongTon => grams / 1_016_046.9088m,

            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        return new Weight(NumberFormatter.RoundForDisplay(converted), unit);
    }
}