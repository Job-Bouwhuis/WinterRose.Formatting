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

    /// <summary>
    /// Formats the given decimal value as a percentage string with the specified number of decimal places.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimals"></param>
    /// <returns></returns>
    public static string Percentage(decimal value, int decimals = 2)
    {
        decimal percentage = value * 100m;

        if (decimals <= 0)
            return $"{percentage:0}%";

        string decimalPlaces = new('0', decimals);
        string format = $"0.{decimalPlaces}";

        return $"{percentage.ToString(format)}%";
    }

    /// <summary>
    /// Formats the given integer value as an ordinal string (e.g., 1st, 2nd, 3rd, 4th).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string Ordinal(int value)
    {
        int absoluteValue = Math.Abs(value);
        int lastTwoDigits = absoluteValue % 100;

        string suffix = lastTwoDigits switch
        {
            11 or 12 or 13 => "th",
            _ => (absoluteValue % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            }
        };

        return $"{value}{suffix}";
    }

    /// <summary>
    /// Formats a decimal value as a fraction, choosing the smallest denominator
    /// that provides a sufficiently accurate representation of the value.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>The fractional representation of the value.</returns>
    public static string Fraction(decimal value)
    {
        if (value == 0)
            return "0";

        bool negative = value < 0;
        value = Math.Abs(value);

        const int MAX_DENOMINATOR = 100000;

        decimal bestDifference = decimal.MaxValue;
        long bestNumerator = 0;
        long bestDenominator = 1;

        for (long denominator = 1; denominator <= MAX_DENOMINATOR; denominator++)
        {
            long numerator = (long)Math.Round(
                value * denominator,
                MidpointRounding.AwayFromZero);

            decimal approximation = (decimal)numerator / denominator;
            decimal difference = Math.Abs(value - approximation);

            if (difference >= bestDifference)
                continue;

            bestDifference = difference;
            bestNumerator = numerator;
            bestDenominator = denominator;

            if (difference == 0)
                break;
        }

        long divisor = GreatestCommonDivisor(
            Math.Abs(bestNumerator),
            bestDenominator);

        bestNumerator /= divisor;
        bestDenominator /= divisor;

        string result = bestDenominator == 1
            ? bestNumerator.ToString()
            : $"{bestNumerator}/{bestDenominator}";

        return negative ? $"-{result}" : result;
    }

    /// <summary>
    /// Formats a number using scientific notation.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="decimals">The number of decimal places to include.</param>
    /// <returns>The scientific representation of the value.</returns>
    public static string Scientific(decimal value, int decimals = 2)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(decimals);

        return value.ToString($"E{decimals}");
    }

    private static long GreatestCommonDivisor(long first, long second)
    {
        while (second != 0)
        {
            long remainder = first % second;
            first = second;
            second = remainder;
        }

        return Math.Abs(first);
    }

    public static decimal RoundForDisplay(decimal value, int significantDigits = 2)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(significantDigits);

        if (value == 0)
            return 0;

        decimal absoluteValue = Math.Abs(value);

        if (absoluteValue >= 1)
            return Math.Round(value, significantDigits);

        int leadingZeroes = 0;
        decimal scaled = absoluteValue;

        while (scaled < 1)
        {
            scaled *= 10;
            leadingZeroes++;
        }

        return Math.Round(value, leadingZeroes + significantDigits - 1);
    }
}
