namespace WinterRose.Formatting.Currency;

/// <summary>
/// 
/// </summary>
public static class CurrencyConverter
{
    public static Money Convert(decimal value, Currency from, Currency to)
    {
        decimal rate = CurrencyExchange.GetConversion(from, to).GetAwaiter().GetResult();
        return new Money(value * rate, to);
    }

    public static async Task<Money> ConvertAsync(decimal value, Currency from, Currency to)
    {
        decimal rate = await CurrencyExchange.GetConversion(from, to);
        return new Money(value * rate, to);
    }
}