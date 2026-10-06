using System.Globalization;
using System.Text;

namespace WinterRose.Formatting;

/// <summary>
/// Provides methods to format numbers
/// </summary>
public static class NumberFormatter
{
    private static readonly (decimal Divisor, string Short, string Long)[] Magnitudes =
    [
        (1_000_000_000_000_000_000M, "E", "quintillion"),
        (1_000_000_000_000_000M,     "P", "quadrillion"),
        (1_000_000_000_000M,         "T", "trillion"),
        (1_000_000_000M,             "B", "billion"),
        (1_000_000M,                 "M", "million"),
        (1_000M,                     "K", "thousand")
    ];

    /// <summary>
    /// Formats a number using compact suffix notation.
    /// </summary>
    /// <example>
    /// 1532 -> 1.53K
    /// 2500000 -> 2.5M
    /// </example>
    public static string Compact(
        decimal value,
        int decimalPlaces = 2)
    {
        decimal absolute = Math.Abs(value);

        foreach ((decimal divisor, string suffix, _) in Magnitudes)
        {
            if (absolute >= divisor)
            {
                decimal scaled = value / divisor;

                return scaled.ToString(
                    $"0.{new string('#', decimalPlaces)}",
                    CultureInfo.InvariantCulture) + suffix;
            }
        }

        return value.ToString(
            CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Formats a number using human-readable magnitude names.
    /// </summary>
    /// <example>
    /// 1532 -> 1.53 thousand
    /// 2500000 -> 2.5 million
    /// </example>
    public static string Human(
        decimal value,
        int decimalPlaces = 2)
    {
        decimal absolute = Math.Abs(value);

        foreach ((decimal divisor, _, string name) in Magnitudes)
        {
            if (absolute >= divisor)
            {
                decimal scaled = value / divisor;

                return
                    $"{scaled.ToString($"0.{new string('#', decimalPlaces)}", CultureInfo.InvariantCulture)} {name}";
            }
        }

        return value.ToString(
            CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Formats a byte count as a human-readable file size.
    /// </summary>
    /// <example>
    /// 1536 -> 1.5 KB
    /// 1073741824 -> 1 GB
    /// </example>
    public static string FileSize(
        long bytes,
        int decimalPlaces = 2,
        bool binary = false)
    {
        string[] units = binary
            ? ["B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB"]
            : ["B", "KB", "MB", "GB", "TB", "PB", "EB"];

        double divisor = binary
            ? 1024
            : 1000;

        double value = bytes;

        int unit = 0;

        while (Math.Abs(value) >= divisor &&
               unit < units.Length - 1)
        {
            value /= divisor;
            unit++;
        }

        return
            $"{Math.Round(value, decimalPlaces).ToString(CultureInfo.InvariantCulture)} {units[unit]}";
    }

    /// <summary>
    /// Converts an integer to a Roman numeral.
    /// </summary>
    /// <remarks>
    /// Supports values from 1 through 3999.
    /// </remarks>
    public static string Roman(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 3999);

        StringBuilder result = new();

        (int Value, string Symbol)[] numerals =
        [
            (1000, "M"),
            (900, "CM"),
            (500, "D"),
            (400, "CD"),
            (100, "C"),
            (90, "XC"),
            (50, "L"),
            (40, "XL"),
            (10, "X"),
            (9, "IX"),
            (5, "V"),
            (4, "IV"),
            (1, "I")
        ];

        foreach ((int numeralValue, string symbol) in numerals)
        {
            while (value >= numeralValue)
            {
                result.Append(symbol);
                value -= numeralValue;
            }
        }

        return result.ToString();
    }
}
