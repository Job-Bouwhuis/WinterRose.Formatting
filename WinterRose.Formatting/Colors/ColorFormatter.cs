using System.Drawing;
using System.Globalization;

namespace WinterRose.Formatting.Colors;

/// <summary>
/// Provides methods to format color values into readable string representations
/// </summary>
public static class ColorFormatter
{
    /// <summary>
    /// Formats the color into the specified kind
    /// </summary>
    /// <param name="color"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static string Format(Color color, ColorFormatKind kind) => Format(
        color.R,
        color.G,
        color.B,
        kind);

    /// <summary>
    /// Formats the <paramref name="color"/> with its alpha component into the specified kind
    /// </summary>
    /// <param name="color"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static string FormatA(Color color, ColorFormatKind kind) => Format(
            color.R,
            color.G,
            color.B,
            color.A,
            kind);

    /// <summary>
    /// Formats the rgb components into the specified kind
    /// </summary>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static string Format(
        byte red,
        byte green,
        byte blue,
        ColorFormatKind kind)
    {
        return Format(
            red,
            green,
            blue,
            255,
            kind);
    }

    /// <summary>
    /// Formats the rgba components into the specified kind
    /// </summary>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    /// <param name="alpha"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string Format(
        byte red,
        byte green,
        byte blue,
        byte alpha,
        ColorFormatKind kind)
    {
        return kind switch
        {
            ColorFormatKind.Hex => $"#{red:X2}{green:X2}{blue:X2}",

            ColorFormatKind.HexWithAlpha => $"#{red:X2}{green:X2}{blue:X2}{alpha:X2}",

            ColorFormatKind.Rgb => $"rgb({red}, {green}, {blue})",

            ColorFormatKind.Rgba => $"rgba({red}, {green}, {blue}, {FormatAlpha(alpha)})",

            ColorFormatKind.Hsl => FormatHsl(red, green, blue),

            ColorFormatKind.Hsla => FormatHsla(red, green, blue, alpha),

            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
    }

    private static string FormatHsl(
        byte red,
        byte green,
        byte blue)
    {
        (double h, double s, double l) = RgbToHsl(red, green, blue);

        return string.Create(
            CultureInfo.InvariantCulture,
            $"hsl({Math.Round(h)}, {Math.Round(s)}%, {Math.Round(l)}%)");
    }

    private static string FormatHsla(
        byte red,
        byte green,
        byte blue,
        byte alpha)
    {
        (double h, double s, double l) = RgbToHsl(red, green, blue);

        return string.Create(
            CultureInfo.InvariantCulture,
            $"hsla({Math.Round(h)}, {Math.Round(s)}%, {Math.Round(l)}%, {FormatAlpha(alpha)})");
    }

    private static string FormatAlpha(
        byte alpha)
    {
        return (alpha / 255d).ToString("0.###", CultureInfo.InvariantCulture);
    }

    private static (double H, double S, double L) RgbToHsl(byte red, byte green, byte blue)
    {
        double r = red / 255d;
        double g = green / 255d;
        double b = blue / 255d;

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));

        double h = 0;
        double s;
        double l = (max + min) / 2;

        if (max == min)
        {
            s = 0;
        }
        else
        {
            double delta = max - min;

            s = l > 0.5
                ? delta / (2 - max - min)
                : delta / (max + min);

            if (max == r)
                h = (g - b) / delta + (g < b ? 6 : 0);
            else if (max == g)
                h = (b - r) / delta + 2;
            else
                h = (r - g) / delta + 4;

            h /= 6;
        }

        return (h * 360, s * 100, l * 100);
    }
}