namespace WinterRose.Formatting.Units;

public readonly record struct Volume(decimal Value, VolumeUnit Unit)
{
    public Volume To(VolumeUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal milliliters = Unit switch
        {
            VolumeUnit.Milliliter => Value,
            VolumeUnit.Liter => Value * 1000m,

            VolumeUnit.UsFluidOunce => Value * 29.5735295625m,
            VolumeUnit.ImperialFluidOunce => Value * 28.4130625m,

            VolumeUnit.UsCup => Value * 236.5882365m,
            VolumeUnit.ImperialCup => Value * 284.131m,

            VolumeUnit.UsPint => Value * 473.176473m,
            VolumeUnit.ImperialPint => Value * 568.26125m,

            VolumeUnit.UsQuart => Value * 946.352946m,
            VolumeUnit.ImperialQuart => Value * 1136.5225m,

            VolumeUnit.UsGallon => Value * 3785.411784m,
            VolumeUnit.ImperialGallon => Value * 4546.09m,

            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            VolumeUnit.Milliliter => milliliters,
            VolumeUnit.Liter => milliliters / 1000m,

            VolumeUnit.UsFluidOunce => milliliters / 29.5735295625m,
            VolumeUnit.ImperialFluidOunce => milliliters / 28.4130625m,

            VolumeUnit.UsCup => milliliters / 236.5882365m,
            VolumeUnit.ImperialCup => milliliters / 284.131m,

            VolumeUnit.UsPint => milliliters / 473.176473m,
            VolumeUnit.ImperialPint => milliliters / 568.26125m,

            VolumeUnit.UsQuart => milliliters / 946.352946m,
            VolumeUnit.ImperialQuart => milliliters / 1136.5225m,

            VolumeUnit.UsGallon => milliliters / 3785.411784m,
            VolumeUnit.ImperialGallon => milliliters / 4546.09m,

            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        return new Volume(NumberFormatter.RoundForDisplay(converted), unit);
    }
}