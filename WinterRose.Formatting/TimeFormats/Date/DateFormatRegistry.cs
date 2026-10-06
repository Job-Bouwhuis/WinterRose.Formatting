using WinterRose.Formatting.TimeFormats;

internal static class DateFormatRegistry
{
    private static readonly Dictionary<string, Func<string, DateFormatNode>> Formatters =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["relative"] = ParseRelative,
            ["date"] = format => new DateNode(format),
            ["time"] = format => new TimeNode(format),
            ["datetime"] = format => new DateTimeNode(format),
        };

    public static bool TryCreateFormatter(
        string name,
        string argument,
        out DateFormatNode? node)
    {
        if (Formatters.TryGetValue(name, out Func<string, DateFormatNode>? formatter))
        {
            node = formatter(argument);
            return true;
        }

        node = null;
        return false;
    }

    public static void RegisterFormatter(
        string name,
        Func<string, DateFormatNode> formatter)
    {
        Formatters[name] = formatter;
    }

    private static DateFormatNode ParseRelative(string options)
    {
        bool shortFormat = false;
        bool calendar = true;

        if (string.IsNullOrWhiteSpace(options))
            return new RelativeNode(false, true);

        foreach (string option in options.Split(
                     ',',
                     StringSplitOptions.RemoveEmptyEntries |
                     StringSplitOptions.TrimEntries))
        {
            switch (option.ToLowerInvariant())
            {
                case "short":
                    shortFormat = true;
                    break;

                case "no-calendar":
                    calendar = false;
                    break;

                default:
                    throw new FormatException(
                        $"Unknown relative option '{option}'.");
            }
        }

        return new RelativeNode(shortFormat, calendar);
    }
}