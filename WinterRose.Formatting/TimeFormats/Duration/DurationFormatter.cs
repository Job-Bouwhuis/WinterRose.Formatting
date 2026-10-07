using System.Globalization;
using System.Text;

namespace WinterRose.Formatting.TimeFormats;

/// <summary>
/// Provides methods for formatting <see cref="TimeSpan"/> values using the
/// DurationFormat expression language.
/// </summary>
/// <remarks>
/// <para>
/// DurationFormat provides a compact formatting syntax for converting
/// <see cref="TimeSpan"/> values into numeric, abbreviated, or human-readable
/// duration strings.
/// </para>
/// <para>
/// Format expressions consist of one or more duration components. Each component
/// represents a duration unit and optionally defines fractional precision,
/// padding behavior, display style, and conditional omission rules.
/// </para>
/// <para>
/// The following duration units are supported:
/// </para>
/// <list type="table">
/// <listheader>
/// <term>Token</term>
/// <description>Unit</description>
/// </listheader>
/// <item>
/// <term><c>d</c></term>
/// <description>Days</description>
/// </item>
/// <item>
/// <term><c>h</c></term>
/// <description>Hours</description>
/// </item>
/// <item>
/// <term><c>m</c></term>
/// <description>Minutes</description>
/// </item>
/// <item>
/// <term><c>s</c></term>
/// <description>Seconds</description>
/// </item>
/// <item>
/// <term><c>ms</c></term>
/// <description>Milliseconds</description>
/// </item>
/// <item>
/// <term><c>us</c></term>
/// <description>Microseconds</description>
/// </item>
/// <item>
/// <term><c>ns</c></term>
/// <description>Nanoseconds</description>
/// </item>
/// </list>
/// <para>
/// Repeating a single-character unit enables two-digit padding.
/// For example:
/// </para>
/// <list type="bullet">
///     <item><description><c>h</c> formats as <c>1</c></description></item>
///     <item><description><c>hh</c> formats as <c>01</c></description></item>
/// </list>
/// <para>
///     Components may specify fractional precision using a decimal point followed
///     by one or more fractional format characters.
/// </para>
/// <list type="table">
///     <listheader>
///         <term>Format</term>
///         <description>Behavior</description>
///     </listheader>
///     <item>
///         <term><c>s.f</c></term>
///         <description>One required fractional digit</description>
///     </item>
///     <item>
///         <term><c>s.fff</c></term>
///         <description>Three required fractional digits</description>
///     </item>
///     <item>
///         <term><c>s.FFF</c></term>
///         <description>
///             Up to three fractional digits with trailing zeroes removed
///         </description>
///     </item>
/// </list>
/// <para>
///     Components may optionally append unit names to the formatted numeric value.
/// </para>
/// <list type="table">
///     <listheader>
///         <term>Suffix</term>
///         <description>Output</description>
///     </listheader>
///     <item>
///         <term><c>+</c></term>
///         <description>Abbreviated unit name such as <c>h</c>, <c>ms</c>, or <c>ns</c></description>
///     </item>
///     <item>
///         <term><c>++</c></term>
///         <description>Full unit name such as <c>hour</c>, <c>hours</c>, or <c>milliseconds</c></description>
///     </item>
/// </list>
/// <para>
///     Components enclosed in square brackets are optional and are omitted when the
///     represented duration value is zero.
/// </para>
/// <para>
///     Examples:
/// </para>
/// <code>
/// hhmmss
///
/// h+
///
/// h++
///
/// s.fff
///
/// ms.FFF+
///
/// [h++][m++][s++]
/// </code>
/// <para>
/// Optional components are evaluated independently. If an optional component
/// resolves to zero for its duration unit, that component is removed from the
/// final output.
/// </para>
/// <para>
/// The formatter evaluates each component directly against the supplied
/// <see cref="TimeSpan"/> value. Components do not consume or subtract values
/// from other components. For example, formatting a duration of 90 minutes
/// using <c>h++m++</c> produces total-hour and total-minute representations
/// rather than a decomposed clock-style duration.
/// </para>
/// </remarks>
public static class DurationFormatter
{
    /// <summary>
    /// Formats a <see cref="TimeSpan"/> value using the specified duration
    /// format expression.
    /// </summary>
    /// <param name="value">
    /// The <see cref="TimeSpan"/> value to format.
    /// </param>
    /// <param name="format">
    /// The duration format expression to apply to <paramref name="value"/>.
    /// The expression may contain duration units, fractional precision
    /// specifiers, optional components, padding directives, and unit-name
    /// modifiers.
    /// </param>
    /// <returns>
    /// A formatted string representation of <paramref name="value"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="format"/> is <see langword="null"/>,
    /// empty, or consists only of whitespace.
    /// </exception>
    /// <exception cref="FormatException">
    /// Thrown when <paramref name="format"/> contains invalid syntax,
    /// unrecognized duration units, unsupported fractional precision,
    /// malformed optional components, or otherwise invalid format expressions.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Format expressions are parsed from left to right and may contain one or
    /// more duration components.
    /// </para>
    /// <para>
    /// Each component consists of:
    /// </para>
    /// <list type="bullet">
    /// <item><description>A duration unit such as <c>h</c> or <c>ms</c></description></item>
    /// <item><description>Optional padding through repeated units</description></item>
    /// <item><description>Optional fractional precision using <c>.f</c> or <c>.F</c></description></item>
    /// <item><description>Optional abbreviated unit suffix using <c>+</c></description></item>
    /// <item><description>Optional full unit name suffix using <c>++</c></description></item>
    /// </list>
    /// <para>
    /// Components enclosed in [square brackets] become optional and are omitted
    /// when the corresponding duration value evaluates to zero.
    /// </para>
    /// <para>
    /// Fractional precision limits depend on the selected duration unit.
    /// Attempting to request more precision than the unit supports results in a
    /// <see cref="FormatException"/>.
    /// </para>
    /// <example>
    /// <code>
    /// string result = DurationFormat.Format(
    ///     TimeSpan.FromMinutes(90),
    ///     "h+");
    ///
    /// // result: "1.5h"
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// string result = DurationFormat.Format(
    ///     TimeSpan.FromSeconds(1.2345),
    ///     "s.fff");
    ///
    /// // result: "1.234"
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// string result = DurationFormat.Format(
    ///     TimeSpan.FromHours(2),
    ///     "h++");
    ///
    /// // result: "2 hours"
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// string result = DurationFormat.Format(
    ///     TimeSpan.FromSeconds(0),
    ///     "[h++][m++][s++]");
    ///
    /// // result: ""
    /// </code>
    /// </example>
    /// </remarks>
    public static string Format(TimeSpan value, string format)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(format);

        IReadOnlyList<DurationFormatToken> tokens = Parse(format);
        return FormatTokens(value, tokens);
    }

    private static IReadOnlyList<DurationFormatToken> Parse(string format)
    {
        List<DurationFormatToken> tokens = [];

        for (int position = 0; position < format.Length;)
        {
            if (format[position] == '\\')
            {
                if (position + 1 >= format.Length)
                    throw new FormatException(
                        "A trailing backslash must escape a character.");

                tokens.Add(new DurationFormatToken(
                    DurationUnit.Days,
                    0,
                    false,
                    0,
                    false,
                    false,
                    false,
                    format[position + 1].ToString()));

                position += 2;
                continue;
            }

            if (format[position] == '[')
            {
                int end = FindOptionalEnd(format, position + 1);

                string component = format[(position + 1)..end];
                DurationFormatToken token = ParseToken(component, true);

                position = end + 1;

                if (position < format.Length)
                {
                    if (format[position] == '\\')
                    {
                        if (position + 1 >= format.Length)
                            throw new FormatException(
                                "A trailing backslash must escape a character.");

                        tokens.Add(new DurationFormatToken(
                            DurationUnit.Days,
                            0,
                            false,
                            0,
                            false,
                            false,
                            false,
                            format[position + 1].ToString()));

                        position += 2;
                    }
                    else
                    {
                        token = token with
                        {
                            TrailingLiteral = format[position].ToString()
                        };

                        position++;
                    }
                }

                tokens.Add(token);
                continue;
            }

            if (TryReadToken(format, ref position, out DurationFormatToken? t))
            {
                tokens.Add(t);
                continue;
            }

            tokens.Add(new DurationFormatToken(
                DurationUnit.Days,
                0,
                false,
                0,
                false,
                false,
                false,
                format[position].ToString()));

            position++;
        }

        return tokens;
    }

    private static bool TryReadToken(string format, ref int position, out DurationFormatToken? token)
    {
        token = null;

        DurationUnit? unit = TryReadUnit(format, ref position);

        if (unit is null)
            return false;

        int padding = 1;

        if (position < format.Length)
        {
            DurationUnit? repeatedUnit = TryReadUnit(format, ref position);

            if (repeatedUnit == unit)
                padding = 2;
            else if (repeatedUnit is not null)
                position -= GetUnitLength(repeatedUnit.Value);
        }

        int fractionDigits = 0;
        bool trimFractionZeroes = false;

        if (position < format.Length && format[position] == '.')
        {
            position++;

            int fractionStart = position;

            while (position < format.Length &&
                   (format[position] == 'f' || format[position] == 'F'))
            {
                if (format[position] == 'F')
                    trimFractionZeroes = true;

                fractionDigits++;
                position++;
            }

            if (fractionDigits == 0)
                throw new FormatException(
                    $"Expected fractional precision after '.' at position {fractionStart}.");
        }

        bool abbreviated = false;
        bool longName = false;

        if (position < format.Length && format[position] == '+')
        {
            abbreviated = true;
            position++;

            if (position < format.Length && format[position] == '+')
            {
                abbreviated = false;
                longName = true;
                position++;
            }
        }

        ValidatePrecision(unit.Value, fractionDigits);

        token = new DurationFormatToken(
            unit.Value,
            fractionDigits,
            trimFractionZeroes,
            padding,
            abbreviated,
            longName,
            false);

        return true;
    }

    private static DurationFormatToken ParseToken(string component, bool optional)
    {
        int position = 0;

        if (!TryReadToken(component, ref position, out DurationFormatToken? token))
            throw new FormatException($"Invalid duration component '{component}'.");

        if (position != component.Length)
            throw new FormatException(
                $"Unexpected character '{component[position]}' in duration component '{component}'.");

        return token with { Optional = optional };
    }

    private static DurationUnit? TryReadUnit(string format, ref int position)
    {
        if (position >= format.Length)
            return null;

        if (position + 1 < format.Length)
        {
            string twoCharacterUnit = format.Substring(position, 2);

            DurationUnit? result = twoCharacterUnit switch
            {
                "ms" => DurationUnit.Milliseconds,
                "us" => DurationUnit.Microseconds,
                "ns" => DurationUnit.Nanoseconds,
                _ => null
            };

            if (result is not null)
            {
                position += 2;
                return result;
            }
        }

        DurationUnit? singleCharacterResult = format[position] switch
        {
            'd' => DurationUnit.Days,
            'h' => DurationUnit.Hours,
            'm' => DurationUnit.Minutes,
            's' => DurationUnit.Seconds,
            _ => null
        };

        if (singleCharacterResult is not null)
            position++;

        return singleCharacterResult;
    }

    private static int GetUnitLength(DurationUnit unit)
    {
        return unit switch
        {
            DurationUnit.Milliseconds => 2,
            DurationUnit.Microseconds => 2,
            DurationUnit.Nanoseconds => 2,
            _ => 1
        };
    }

    private static void ValidatePrecision(DurationUnit unit, int fractionDigits)
    {
        int maximum = unit switch
        {
            DurationUnit.Days => 7,
            DurationUnit.Hours => 7,
            DurationUnit.Minutes => 7,
            DurationUnit.Seconds => 7,
            DurationUnit.Milliseconds => 4,
            DurationUnit.Microseconds => 1,
            DurationUnit.Nanoseconds => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };

        if (fractionDigits > maximum)
        {
            throw new FormatException(
                $"The duration unit '{GetUnitName(unit)}' supports a maximum of " +
                $"{maximum} fractional digits.");
        }
    }

    private static string FormatTokens(TimeSpan value, IReadOnlyList<DurationFormatToken> tokens)
    {
        StringBuilder result = new();
        TimeSpan remaining = value;

        foreach (DurationFormatToken token in tokens)
        {
            if (token.Literal is not null)
            {
                result.Append(token.Literal);
                continue;
            }

            bool hidden = token.Optional && IsZero(remaining, token.Unit);

            if (hidden)
                continue;

            result.Append(FormatToken(remaining, token));

            remaining -= GetConsumedDuration(remaining, token.Unit);
        }

        return result.ToString();
    }

    private static TimeSpan GetConsumedDuration(TimeSpan value, DurationUnit unit)
    {
        long ticks = value.Ticks;

        return unit switch
        {
            DurationUnit.Days =>
                TimeSpan.FromTicks(
                    ticks / TimeSpan.TicksPerDay * TimeSpan.TicksPerDay),

            DurationUnit.Hours =>
                TimeSpan.FromTicks(
                    ticks / TimeSpan.TicksPerHour * TimeSpan.TicksPerHour),

            DurationUnit.Minutes =>
                TimeSpan.FromTicks(
                    ticks / TimeSpan.TicksPerMinute * TimeSpan.TicksPerMinute),

            DurationUnit.Seconds =>
                TimeSpan.FromTicks(
                    ticks / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond),

            DurationUnit.Milliseconds =>
                TimeSpan.FromTicks(
                    ticks / TimeSpan.TicksPerMillisecond * TimeSpan.TicksPerMillisecond),

            DurationUnit.Microseconds =>
                TimeSpan.FromTicks(
                    ticks / 10 * 10),

            DurationUnit.Nanoseconds =>
                TimeSpan.FromTicks(
                    ticks),

            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }

    private static int FindOptionalEnd(
    string format,
    int start)
    {
        for (int position = start; position < format.Length; position++)
        {
            if (format[position] == '\\')
            {
                position++;
                continue;
            }

            if (format[position] == ']')
                return position;
        }

        throw new FormatException(
            "An optional duration component was not closed.");
    }

    private static string FormatToken(
        TimeSpan value,
        DurationFormatToken token)
    {
        decimal amount = GetAmount(value, token.Unit);

        string numeric = FormatNumeric(
            amount,
            token.FractionDigits,
            token.TrimFractionZeroes,
            token.Padding);

        if (token.LongName)
        {
            string name = GetUnitName(token.Unit, amount);
            return $"{numeric} {name}";
        }

        if (token.Abbreviated)
            return $"{numeric}{GetUnitAbbreviation(token.Unit)}";

        return numeric;
    }

    private static decimal GetAmount(
        TimeSpan value,
        DurationUnit unit)
    {
        return unit switch
        {
            DurationUnit.Days => (decimal)value.Ticks / TimeSpan.TicksPerDay,
            DurationUnit.Hours => (decimal)value.Ticks / TimeSpan.TicksPerHour,
            DurationUnit.Minutes => (decimal)value.Ticks / TimeSpan.TicksPerMinute,
            DurationUnit.Seconds => (decimal)value.Ticks / TimeSpan.TicksPerSecond,
            DurationUnit.Milliseconds => (decimal)value.Ticks / TimeSpan.TicksPerMillisecond,
            DurationUnit.Microseconds => (decimal)value.Ticks / 10,
            DurationUnit.Nanoseconds => (decimal)value.Ticks * 100,
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }

    private static string FormatNumeric(
        decimal amount,
        int fractionDigits,
        bool trimFractionZeroes,
        int padding)
    {
        decimal absoluteAmount = Math.Abs(amount);
        decimal wholePart = decimal.Truncate(absoluteAmount);
        decimal fractionalPart = absoluteAmount - wholePart;

        string whole = wholePart.ToString(
            "0",
            CultureInfo.InvariantCulture);

        if (padding > 1)
            whole = whole.PadLeft(padding, '0');

        if (fractionDigits == 0)
            return amount < 0 ? $"-{whole}" : whole;

        decimal scale = 1;

        for (int index = 0; index < fractionDigits; index++)
            scale *= 10;

        int fractionalValue = (int)decimal.Truncate(fractionalPart * scale);

        string fraction = fractionalValue.ToString(
            $"D{fractionDigits}",
            CultureInfo.InvariantCulture);

        if (trimFractionZeroes)
            fraction = fraction.TrimEnd('0');

        string result = fraction.Length == 0
            ? whole
            : $"{whole}.{fraction}";

        return amount < 0 ? $"-{result}" : result;
    }

    private static bool IsZero(
        TimeSpan value,
        DurationUnit unit)
    {
        return Math.Floor(GetAmount(value, unit)) == 0;
    }

    private static string GetUnitAbbreviation(DurationUnit unit)
    {
        return unit switch
        {
            DurationUnit.Days => "d",
            DurationUnit.Hours => "h",
            DurationUnit.Minutes => "m",
            DurationUnit.Seconds => "s",
            DurationUnit.Milliseconds => "ms",
            DurationUnit.Microseconds => "us",
            DurationUnit.Nanoseconds => "ns",
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }

    private static string GetUnitName(
        DurationUnit unit,
        decimal amount)
    {
        bool singular = amount == 1;

        return unit switch
        {
            DurationUnit.Days => singular ? "day" : "days",
            DurationUnit.Hours => singular ? "hour" : "hours",
            DurationUnit.Minutes => singular ? "minute" : "minutes",
            DurationUnit.Seconds => singular ? "second" : "seconds",
            DurationUnit.Milliseconds => singular ? "millisecond" : "milliseconds",
            DurationUnit.Microseconds => singular ? "microsecond" : "microseconds",
            DurationUnit.Nanoseconds => singular ? "nanosecond" : "nanoseconds",
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }

    private static string GetUnitName(DurationUnit unit)
    {
        return unit switch
        {
            DurationUnit.Days => "days",
            DurationUnit.Hours => "hours",
            DurationUnit.Minutes => "minutes",
            DurationUnit.Seconds => "seconds",
            DurationUnit.Milliseconds => "milliseconds",
            DurationUnit.Microseconds => "microseconds",
            DurationUnit.Nanoseconds => "nanoseconds",
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }
}