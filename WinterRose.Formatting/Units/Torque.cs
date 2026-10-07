namespace WinterRose.Formatting.Units;

public readonly record struct Torque(decimal Value, TorqueUnit Unit)
{
    public Torque To(TorqueUnit unit)
    {
        if (Unit == unit)
            return this;

        decimal newtonMeters = Unit switch
        {
            TorqueUnit.NewtonMeter => Value,
            TorqueUnit.KilonewtonMeter => Value * 1000m,
            TorqueUnit.PoundFoot => Value * 1.3558179483314004m,
            TorqueUnit.PoundInch => Value * 0.1129848290276167m,
            _ => throw new ArgumentOutOfRangeException()
        };

        decimal converted = unit switch
        {
            TorqueUnit.NewtonMeter => newtonMeters,
            TorqueUnit.KilonewtonMeter => newtonMeters / 1000m,
            TorqueUnit.PoundFoot => newtonMeters / 1.3558179483314004m,
            TorqueUnit.PoundInch => newtonMeters / 0.1129848290276167m,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        return new Torque(NumberFormatter.RoundForDisplay(converted), unit);
    }
}
