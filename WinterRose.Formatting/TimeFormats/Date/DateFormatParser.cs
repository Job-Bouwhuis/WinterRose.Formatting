namespace WinterRose.Formatting.TimeFormats;

public sealed class DateFormatParser
{
    public DateFormatNode Parse(string format)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(format);

        int position = 0;
        DateFormatNode result = ParseFormat(format, ref position);

        SkipWhitespace(format, ref position);

        if (position != format.Length)
            throw new FormatException($"Unexpected character '{format[position]}' at position {position}.");

        return result;
    }

    private static DateFormatNode ParseFormat(string format, ref int position)
    {
        SkipWhitespace(format, ref position);

        int questionMark = FindTopLevelCharacter(format, position, '?');

        if (questionMark >= 0)
        {
            string conditionText = format[position..questionMark].Trim();
            position = questionMark + 1;

            DateCondition condition = ParseCondition(conditionText);
            DateFormatNode whenTrue = ParseFormat(format, ref position);

            SkipWhitespace(format, ref position);

            if (position >= format.Length || format[position] != ':')
                throw new FormatException("Conditional format is missing ':'.");

            position++;

            DateFormatNode whenFalse = ParseFormat(format, ref position);

            return new ConditionalNode(condition, whenTrue, whenFalse);
        }

        return ParseFormatter(format, ref position);
    }

    private static DateFormatNode ParseFormatter(string format, ref int position)
    {
        string name = ReadIdentifier(format, ref position);

        if (name.Equals("relative", StringComparison.OrdinalIgnoreCase))
            return ParseRelative(format, ref position);

        if (name.Equals("date", StringComparison.OrdinalIgnoreCase))
            return new DateNode(ReadOptionalFormat(format, ref position));

        if (name.Equals("time", StringComparison.OrdinalIgnoreCase))
            return new TimeNode(ReadOptionalFormat(format, ref position));

        if (name.Equals("datetime", StringComparison.OrdinalIgnoreCase))
            return new DateTimeNode(ReadOptionalFormat(format, ref position));

        throw new FormatException($"Unknown date formatter '{name}'.");
    }

    private static RelativeNode ParseRelative(string format, ref int position)
    {
        bool shortFormat = false;
        bool calendar = true;

        if (position < format.Length && format[position] == '[')
        {
            string options = ReadBracketContent(format, ref position);

            foreach (string option in options.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (option.Equals("short", StringComparison.OrdinalIgnoreCase))
                {
                    shortFormat = true;
                    continue;
                }

                if (option.Equals("no-calendar", StringComparison.OrdinalIgnoreCase))
                {
                    calendar = false;
                    continue;
                }

                throw new FormatException($"Unknown relative option '{option}'.");
            }
        }

        return new RelativeNode(shortFormat, calendar);
    }

    private static DateCondition ParseCondition(string condition)
    {
        condition = condition.Trim();

        foreach ((string prefix, DateComparison comparison) in new[]
        {
            ("<=", DateComparison.LessThanOrEqual),
            (">=", DateComparison.GreaterThanOrEqual),
            ("<", DateComparison.LessThan),
            (">", DateComparison.GreaterThan),
            ("=", DateComparison.Equal)
    })
        {
            if (!condition.StartsWith(prefix, StringComparison.Ordinal))
                continue;

            string durationText = condition[prefix.Length..].Trim();
            return new DurationCondition(
                comparison,
                ParseDuration(durationText));
        }

        return new KeywordCondition(condition);
    }

    private static TimeSpan ParseDuration(string value)
    {
        if (value.Length < 2)
            throw new FormatException($"Invalid duration '{value}'.");

        int numberLength = 0;

        while (numberLength < value.Length && char.IsDigit(value[numberLength]))
            numberLength++;

        if (numberLength == 0)
            throw new FormatException($"Invalid duration '{value}'.");

        if (!double.TryParse(
                value[..numberLength],
                System.Globalization.NumberStyles.None,
                System.Globalization.CultureInfo.InvariantCulture,
                out double amount))
        {
            throw new FormatException($"Invalid duration '{value}'.");
        }

        string unit = value[numberLength..];

        return unit.ToLowerInvariant() switch
        {
            "ms" => TimeSpan.FromMilliseconds(amount),
            "s" => TimeSpan.FromSeconds(amount),
            "m" => TimeSpan.FromMinutes(amount),
            "h" => TimeSpan.FromHours(amount),
            "d" => TimeSpan.FromDays(amount),
            "w" => TimeSpan.FromDays(amount * 7),
            "mo" => TimeSpan.FromDays(amount * 30),
            "y" => TimeSpan.FromDays(amount * 365),
            _ => throw new FormatException($"Unknown duration unit '{unit}'.")
        };
    }

    private static string ReadOptionalFormat(string format, ref int position)
    {
        if (position >= format.Length || format[position] != '[')
            return string.Empty;

        return ReadBracketContent(format, ref position);
    }

    private static string ReadBracketContent(string format, ref int position)
    {
        if (format[position] != '[')
            throw new FormatException("Expected '['.");

        position++;

        int start = position;
        int depth = 1;

        while (position < format.Length)
        {
            if (format[position] == '[')
                depth++;

            else if (format[position] == ']')
            {
                depth--;

                if (depth == 0)
                {
                    string result = format[start..position];
                    position++;
                    return result;
                }
            }

            position++;
        }

        throw new FormatException("Unclosed '['.");
    }

    private static string ReadIdentifier(string format, ref int position)
    {
        SkipWhitespace(format, ref position);

        int start = position;

        while (position < format.Length && char.IsLetter(format[position]))
            position++;

        if (start == position)
            throw new FormatException($"Expected formatter at position {position}.");

        return format[start..position];
    }

    private static int FindTopLevelCharacter(
    string format,
    int position,
    char target)
    {
        bool branch = position > 0 &&
                      format[position - 1] == '?' &&
                      !IsEscaped(format, position - 1);

        int depth = 0;

        for (int index = position; index < format.Length; index++)
        {
            char character = format[index];

            if (character == '\\')
            {
                index++;
                continue;
            }

            if (branch && character == ':')
                return target == ':'
                    ? index
                    : -1;

            if (character == '?')
            {
                if (target == '?' && depth == 0)
                    return index;

                depth++;
                continue;
            }

            if (character == ':')
            {
                if (depth > 0)
                {
                    depth--;
                    continue;
                }

                if (target == ':')
                    return index;

                return -1;
            }

            if (character == target && depth == 0)
                return index;
        }

        return -1;
    }

    private static bool IsEscaped(string value, int position)
    {
        int backslashes = 0;

        for (int index = position - 1;
             index >= 0 && value[index] == '\\';
             index--)
        {
            backslashes++;
        }

        return backslashes % 2 != 0;
    }

    private static void SkipWhitespace(string format, ref int position)
    {
        while (position < format.Length && char.IsWhiteSpace(format[position]))
            position++;
    }
}