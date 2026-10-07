using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WinterRose.Formatting.Currency;

/// <summary>
/// Provides methods to read the currency code and symbol off of the <see cref="Currency"/> enum
/// </summary>
public static class CurrencyExtensions
{
    extension(Currency currency)
    {
        /// <summary>
        /// Gets the code for the currency
        /// </summary>
        /// <returns></returns>
        public string CurrencyCode
        {
            get
            {
                var member = typeof(Currency)
                .GetMember(currency.ToString())
                .Single();

                return member.GetCustomAttribute<DisplayAttribute>()?.Name
                    ?? currency.ToString();
            }
        }

        /// <summary>
        /// Gets the symbol for the currency
        /// </summary>
        /// <returns></returns>
        public string CurrencySymbol
        {
            get
            {
                var member = typeof(Currency)
                .GetMember(currency.ToString())
                .Single();

                return member
                    .GetCustomAttribute<DisplayAttribute>()
                    ?.Description
                    ?? string.Empty;
            }
        }
    }
}