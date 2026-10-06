using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WinterRose.Formatting.TimeFormats;

internal sealed class DateFormatParser
{
    public DateFormatNode Parse(string format)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(format);

        int position = 0;
        DateFormatNode result = ParseFormat(format, ref position, true);

        SkipWhitespace(format, ref position);

        if (position != format.Length)
        {
            throw new FormatException(
                $"Unexpected character '{format[position]}' at position {position}.");
        }

        return result;
    }

    private static DateFormatNode ParseFormat(
    string format,
    ref int position,
    bool root)
    {
        List<DateFormatNode> nodes = new();

        do
        {
            if (position < format.Length && (format[position] == '\n' || format[position] == '\r'))
            {
                if (position + 1 < format.Length &&
                    (format[position] == '\r' && format[position + 1] == '\n' ||
                     format[position] == '\n' && format[position + 1] == '\r'))
                {
                    position += 2;
                }
                else
                {
                    position++;
                }

                nodes.Add(new TextNode("\n"));
                continue;
            }

            SkipWhitespace(format, ref position);

            char[] terminators = [',', '}', ';'];
            if (position >= format.Length || terminators.Contains(format[position]))
                break;

            nodes.Add(
            ParseFormatter(
                format,
                ref position));
        }
        while (position < format.Length && root);

        if (nodes.Count == 0)
            return new TextNode(string.Empty);

        if (nodes.Count == 1)
            return nodes[0];

        return new SequenceNode(nodes);
    }

    private static DateFormatNode ParseFormatter(string format, ref int position)
    {
        int questionMark = FindTopLevelCharacter(
                format,
                position,
                '?');

        if (questionMark >= 0)
        {
            string conditionText =
                format[position..questionMark].Trim();

            position = questionMark + 1;

            DateCondition condition =
                ParseCondition(conditionText);

            DateFormatNode whenTrue = ParseFormat(format, ref position, false);

            SkipWhitespace(format, ref position);

            if (position >= format.Length || format[position] != ':')
            {
                throw new FormatException("Conditional format is missing ':'.");
            }

            position++;

            DateFormatNode whenFalse = ParseFormat(format, ref position, false);

            return new ConditionalNode(
                    condition,
                    whenTrue,
                    whenFalse);
        }

        string name = ReadIdentifier(
            format,
            ref position);
        ;
        if (position < format.Length && PeekNextNonWhitespace(format, position, '{', out int switchBrace) && switchBrace is not 0)
        {
            position = switchBrace;
            if (!DateVariables.Exists(name.Trim()))
            {
                throw new FormatException(
                    $"Unknown date variable '{name}'.");
            }

            return ParseSwitch(
                format,
                ref position,
                name.Trim());
        }

        string argument = ReadOptionalFormat(
            format,
            ref position);

        if (DateFormatRegistry.TryCreateFormatter(
                name,
                argument,
                out DateFormatNode? node))
        {
            return node;
        }

        if (name.StartsWith('@') && DateVariables.Exists(name[1..]))
            return new VariableNode(name[1..]);

        return new TextNode(name);
    }

    private static DateFormatNode ParseSwitch(
        string format,
        ref int position,
        string variable)
    {
        if (format[position] != '{')
            throw new FormatException("Expected '{'.");

        position++;

        Dictionary<object, DateFormatNode> cases = new();
        DateFormatNode defaultNode = new TextNode(string.Empty);

        while (true)
        {
            SkipWhitespace(format, ref position);

            if (position >= format.Length)
                throw new FormatException("Unclosed switch expression.");

            if (format[position] == '}')
            {
                position++;
                break;
            }

            string caseText = ReadSwitchCaseKey(
                format,
                ref position);

            SkipWhitespace(format, ref position);

            if (position >= format.Length ||
                format[position] != ':')
            {
                throw new FormatException(
                    "Switch case is missing ':'.");
            }

            position++;

            DateFormatNode value = ParseFormat(format, ref position, false);

            if (caseText.Equals(
                    "default",
                    StringComparison.OrdinalIgnoreCase))
            {
                defaultNode = value;
            }
            else
            {
                object caseValue = ParseSwitchCaseValue(caseText);

                if (!cases.TryAdd(caseValue, value))
                {
                    throw new FormatException(
                        $"Duplicate switch case '{caseText}'.");
                }
            }

            SkipWhitespace(format, ref position);

            if (position >= format.Length)
                throw new FormatException(
                    "Unclosed switch expression.");

            if (format[position] == ',')
            {
                position++;
                continue;
            }

            if (format[position] == '}')
            {
                position++;
                break;
            }

            throw new FormatException(
                $"Expected ',' or '}}' in switch expression at position {position}.");
        }

        return new SwitchNode(
            variable,
            cases,
            defaultNode);
    }

    private static string ReadSwitchCaseKey(
    string format,
    ref int position)
    {
        SkipWhitespace(format, ref position);

        if (position >= format.Length)
            throw new FormatException(
                "Expected switch case.");

        int start = position;

        if (format[position] == '"')
        {
            position++;

            while (position < format.Length)
            {
                if (format[position] == '\\')
                {
                    position += 2;
                    continue;
                }

                if (format[position] == '"')
                {
                    position++;

                    return format[start..position];
                }

                position++;
            }

            throw new FormatException(
                "Unterminated quoted switch case.");
        }

        while (position < format.Length &&
               format[position] != ':')
        {
            position++;
        }

        if (position >= format.Length)
            throw new FormatException(
                "Switch case is missing ':'.");

        return format[start..position].Trim();
    }

    private static object ParseSwitchCaseValue(string value)
    {
        value = value.Trim();

        if (value.Length >= 2 &&
            value[0] == '"' &&
            value[^1] == '"')
        {
            return value[1..^1];
        }

        if (int.TryParse(
                value,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out int integer))
        {
            return integer;
        }

        if (double.TryParse(
                value,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out double number))
        {
            return number;
        }

        if (bool.TryParse(value, out bool boolean))
            return boolean;

        return value;
    }

    private static DateCondition ParseCondition(
        string condition)
    {
        condition = condition.Trim();

        if (TryParseDateVariableCondition(
                condition,
                out DatePartCondition? variableCondition))
        {
            return variableCondition;
        }

        if (TryParseDurationCondition(
                condition,
                out DurationCondition? durationCondition))
        {
            return durationCondition;
        }

        if (TryParseKeywordCondition(
                condition,
                out KeywordCondition? keywordCondition))
        {
            return keywordCondition;
        }

        throw new FormatException(
            $"Unknown date condition '{condition}'.");
    }

    private static bool TryParseDateVariableCondition(
        string condition,
        out DatePartCondition? result)
    {
        result = null;

        foreach (string name in DateVariables.Names)
        {
            if (!condition.StartsWith(
                    name,
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string remaining = condition[name.Length..];

            if (!TryParseComparison(
                    remaining,
                    out DateComparison comparison,
                    out string valueText))
            {
                continue;
            }

            result = new DatePartCondition(
                name,
                comparison,
                valueText);

            return true;
        }

        return false;
    }

    private static bool TryParseDurationCondition(
        string condition,
        out DurationCondition? result)
    {
        result = null;

        if (!TryParseComparison(
                condition,
                out DateComparison comparison,
                out string valueText))
        {
            return false;
        }

        result = new DurationCondition(
            comparison,
            ParseDuration(valueText));

        return true;
    }

    private static bool TryParseKeywordCondition(
        string condition,
        out KeywordCondition? result)
    {
        result = condition.ToLowerInvariant() switch
        {
            "past" => new KeywordCondition("past"),
            "future" => new KeywordCondition("future"),
            "today" => new KeywordCondition("today"),
            "tomorrow" => new KeywordCondition("tomorrow"),
            "yesterday" => new KeywordCondition("yesterday"),
            _ => null
        };

        return result is not null;
    }

    private static bool TryParseComparison(
        string value,
        out DateComparison comparison,
        out string valueText)
    {
        if (value.StartsWith("<="))
        {
            comparison = DateComparison.LessThanOrEqual;
            valueText = value[2..].Trim();
            return true;
        }

        if (value.StartsWith(">="))
        {
            comparison = DateComparison.GreaterThanOrEqual;
            valueText = value[2..].Trim();
            return true;
        }

        if (value.StartsWith('<'))
        {
            comparison = DateComparison.LessThan;
            valueText = value[1..].Trim();
            return true;
        }

        if (value.StartsWith('>'))
        {
            comparison = DateComparison.GreaterThan;
            valueText = value[1..].Trim();
            return true;
        }

        if (value.StartsWith('='))
        {
            comparison = DateComparison.Equal;
            valueText = value[1..].Trim();
            return true;
        }

        comparison = default;
        valueText = string.Empty;
        return false;
    }

    private static TimeSpan ParseDuration(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new FormatException(
                "Duration condition requires a duration value.");
        }

        int unitStart = value.Length;

        while (unitStart > 0 &&
               char.IsLetter(value[unitStart - 1]))
        {
            unitStart--;
        }

        if (unitStart == 0 ||
            unitStart == value.Length)
        {
            throw new FormatException(
                $"Invalid duration '{value}'.");
        }

        string amountText = value[..unitStart];
        string unit = value[unitStart..];

        if (!double.TryParse(
                amountText,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out double amount))
        {
            throw new FormatException(
                $"Invalid duration '{value}'.");
        }

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

            _ => throw new FormatException(
                $"Unknown duration unit '{unit}'.")
        };
    }

    private static string ReadOptionalFormat(
        string format,
        ref int position)
    {
        if (position >= format.Length ||
            format[position] != '[')
        {
            return string.Empty;
        }

        return ReadBracketContent(
            format,
            ref position);
    }

    private static string ReadBracketContent(
        string format,
        ref int position)
    {
        if (format[position] != '[')
            throw new FormatException("Expected '['.");

        position++;

        int start = position;
        int depth = 1;

        while (position < format.Length)
        {
            if (format[position] == '[')
            {
                depth++;
            }
            else if (format[position] == ']')
            {
                depth--;

                if (depth == 0)
                {
                    string result =
                        format[start..position];

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

        if (position >= format.Length)
        {
            throw new FormatException(
                $"Expected identifier at position {position}.");
        }

        if (format[position] == '"')
        {
            position++;

            int start = position;
            StringBuilder result = new StringBuilder();

            while (position < format.Length)
            {
                if (format[position] == '\\')
                {
                    position++;

                    if (position < format.Length)
                    {
                        result.Append(format[position]);
                        position++;
                    }
                    continue;
                }

                if (format[position] == '"')
                    break;

                result.Append(format[position]);
                position++;
            }

            if (position >= format.Length)
            {
                throw new FormatException(
                    $"Unterminated quoted identifier starting at position {start - 1}.");
            }

            position++;

            return result.ToString();
        }

        int startPosition = position;
        bool escaped = false;
        bool capturingVariable = false;

        while (position < format.Length)
        {
            char character = format[position];

            if (!escaped && character == '\\')
            {
                escaped = true;
                position++;
                continue;
            }

            if (escaped)
            {
                escaped = false;
                position++;
                continue;
            }

            if (character == '@')
            {
                if (capturingVariable || position > startPosition)
                    break;

                capturingVariable = true;
                position++;
                continue;
            }

            if (capturingVariable)
            {
                if (!char.IsLetterOrDigit(character))
                {
                    break;
                }

                position++;
                continue;
            }

            if (char.IsWhiteSpace(character) || character is ';' or '?' or ':' or
                '[' or ']' or '(' or ')' or
                '{' or '}' or '<' or '>' or
                '=' or ',')
            {
                if (character is not ' ')
                    break;
            }

            position++;
        }

        string identifier = format[startPosition..position];

        if (position < format.Length && format[position] == ';')
            position++;

        return identifier;
    }

    private static int FindTopLevelCharacter(
        string format,
        int position,
        char target)
    {
        bool inQuotes = false;
        int depth = 0;

        for (int index = position;
             index < format.Length;
             index++)
        {
            char character = format[index];

            if (character == '\\')
            {
                index++;
                continue;
            }

            if (character == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (inQuotes)
                continue;

            if (character == '?')
            {
                if (target == '?' &&
                    depth == 0)
                {
                    return index;
                }

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

            if (character == target &&
                depth == 0)
            {
                return index;
            }
        }

        return -1;
    }

    private static DateFormatNode ParseText(string text)
    {
        List<DateFormatNode> nodes = new();
        int textStart = 0;

        for (int position = 0;
             position < text.Length;
             position++)
        {
            if (text[position] == '\\')
            {
                if (position + 1 < text.Length &&
                    text[position + 1] == '@')
                {
                    if (position > textStart)
                    {
                        nodes.Add(
                            new TextNode(
                                text[textStart..position]));
                    }

                    nodes.Add(
                        new TextNode("@"));

                    position++;
                    textStart = position + 1;

                    continue;
                }

                continue;
            }

            if (text[position] != '@')
                continue;

            if (position > textStart)
            {
                nodes.Add(
                    new TextNode(
                        text[textStart..position]));
            }

            position++;

            int variableStart = position;

            while (position < text.Length &&
                   IsVariableCharacter(text[position]))
            {
                position++;
            }

            if (variableStart == position)
            {
                nodes.Add(new TextNode("@"));
                position--;
                textStart = position + 1;
                continue;
            }

            string variableName =
                text[variableStart..position];

            if (!DateVariables.Exists(variableName))
            {
                throw new FormatException(
                    $"Unknown date variable '{variableName}'.");
            }

            nodes.Add(
                new VariableNode(variableName));

            position--;
            textStart = position + 1;
        }

        if (textStart < text.Length)
        {
            nodes.Add(
                new TextNode(
                    text[textStart..]));
        }

        if (nodes.Count == 0)
            return new TextNode(string.Empty);

        if (nodes.Count == 1)
            return nodes[0];

        return new SequenceNode(nodes);
    }

    private static bool IsVariableCharacter(char character) => char.IsLetterOrDigit(character) || character is '_';

    private static void SkipWhitespace(
        string format,
        ref int position)
    {
        while (position < format.Length &&
               char.IsWhiteSpace(format[position]))
        {
            position++;
        }
    }

    private static bool PeekNextNonWhitespace(
        string format,
        int position,
        char expected,
        out int matchedPosition)
    {
        matchedPosition = -1;

        while (position < format.Length &&
               char.IsWhiteSpace(format[position]))
        {
            position++;
        }

        // Check bounds
        if (position >= format.Length)
        {
            return false;
        }

        if (format[position] == expected)
        {
            matchedPosition = position;
            return true;
        }

        return false;
    }
}