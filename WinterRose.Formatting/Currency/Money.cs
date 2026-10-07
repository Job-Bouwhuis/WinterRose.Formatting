using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterRose.Formatting.Currency;

/// <summary>
/// Represents a monitary value in a specific currency
/// </summary>
/// <param name="Amount"></param>
/// <param name="Currency"></param>
[DebuggerDisplay("{ToString()}")]
public readonly record struct Money(decimal Amount, Currency Currency)
{
    /// <summary>"
    /// Converts the Money object to a human readable string. eg: "$50"
    /// </summary>
    /// <returns></returns>
    public override string ToString() => ToCompactString(2);

    /// <summary>
    /// A compact representation of the currency. eg: "5.1k"
    /// </summary>
    /// <param name="decimals"></param>
    /// <returns></returns>
    public string ToCompactString(int decimals) => $"{Currency.CurrencySymbol}{NumberFormatter.Compact(Amount, decimals)}";

    /// <summary>
    /// A human representation of the currency. eg: "5.1 thousand"
    /// </summary>
    /// <param name="decimals"></param>
    /// <returns></returns>
    public string ToHumanString(int decimals) => $"{Currency.CurrencySymbol}{NumberFormatter.Human(Amount, decimals)}";

    /// <summary>
    /// Converts the money to the specified <paramref name="currency"/> and returns a new <see cref="Money"/> instance
    /// </summary>
    /// <param name="currency"></param>
    /// <returns></returns>
    public Money To(Currency currency) => CurrencyConverter.Convert(Amount, Currency, currency);
    /// <summary>
    /// Converts the money to the specified <paramref name="currency"/> and returns a new <see cref="Money"/> instance
    /// </summary>
    /// <param name="currency"></param>
    /// <returns></returns>
    public async ValueTask<Money> ToAsync(Currency currency) => await CurrencyConverter.ConvertAsync(Amount, Currency, currency);


}
