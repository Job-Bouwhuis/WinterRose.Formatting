using System.Globalization;

namespace WinterRose.Formatting.TimeFormats;

/// <summary>
/// Registers 'variables' in <see cref="DateFormatter"/>. for example: "month=12?christmas!:sad times"
/// </summary>
internal static class DateVariables
{
    private static readonly Dictionary<string, Func<DateTime, object>> Variables =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["year"] = date => date.Year,
            ["month"] = date => date.Month,
            ["day"] = date => date.Day,
            ["quarter"] = date => ((date.Month - 1) / 3) + 1,
            ["week"] = date => ISOWeek.GetWeekOfYear(date),
            ["weekday"] = date => date.DayOfWeek.ToString(),
            ["hour"] = date => date.Hour,
            ["minute"] = date => date.Minute,
            ["second"] = date => date.Second,
            ["kind"] = date => date.Kind.ToString(),
            ["milli"] = date => date.Millisecond,
            ["micro"] = date => date.Microsecond,
            ["nano"] = date => date.Nanosecond,
        };

    public static bool Exists(string name)
    {
        return Variables.ContainsKey(name);
    }

    public static object Evaluate(
        string name,
        DateTime value)
    {
        if (!Variables.TryGetValue(name, out Func<DateTime, object>? variable))
        {
            throw new FormatException($"Unknown date variable '{name}'");
        }

        return variable(value);
    }

    public static IEnumerable<string> Names => Variables.Keys;

    public static void Register(
        string name,
        Func<DateTime, object> evaluator)
    {
        Variables[name] = evaluator;
    }
}