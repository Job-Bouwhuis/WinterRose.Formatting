namespace WinterRose.Formatting.Units;

public readonly record struct Energy(decimal Value, EnergyUnit Unit)
{
    public Energy To(EnergyUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal joules = Unit switch
        {
            EnergyUnit.Joule => Value,
            EnergyUnit.Kilojoule => Value * 1000m,
            EnergyUnit.Megajoule => Value * 1_000_000m,

            EnergyUnit.WattHour => Value * 3600m,
            EnergyUnit.KilowattHour => Value * 3_600_000m,
            EnergyUnit.MegawattHour => Value * 3_600_000_000m,

            EnergyUnit.Calorie => Value * 4.184m,
            EnergyUnit.Kilocalorie => Value * 4184m,

            EnergyUnit.BritishThermalUnit => Value * 1055.05585262m,

            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            EnergyUnit.Joule => joules,
            EnergyUnit.Kilojoule => joules / 1000m,
            EnergyUnit.Megajoule => joules / 1_000_000m,

            EnergyUnit.WattHour => joules / 3600m,
            EnergyUnit.KilowattHour => joules / 3_600_000m,
            EnergyUnit.MegawattHour => joules / 3_600_000_000m,

            EnergyUnit.Calorie => joules / 4.184m,
            EnergyUnit.Kilocalorie => joules / 4184m,

            EnergyUnit.BritishThermalUnit => joules / 1055.05585262m,

            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        return new Energy(NumberFormatter.RoundForDisplay(converted), unit);
    }
}
