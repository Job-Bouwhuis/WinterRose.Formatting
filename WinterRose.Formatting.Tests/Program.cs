using Microsoft.VisualBasic;
using WinterRose.Formatting;
using WinterRose.Formatting.Colors;
using WinterRose.Formatting.Paths;
using WinterRose.Formatting.TimeFormats;

Console.WriteLine("Color:");
Console.WriteLine(ColorFormatter.Format(255, 128, 0, ColorFormatKind.Hex));
Console.WriteLine(ColorFormatter.Format(255, 128, 0, ColorFormatKind.Rgb));
Console.WriteLine(ColorFormatter.Format(255, 128, 0, 128, ColorFormatKind.Rgba));
Console.WriteLine(ColorFormatter.Format(255, 128, 0, ColorFormatKind.Hsl));
Console.WriteLine(ColorFormatter.Format(255, 128, 0, 128, ColorFormatKind.Hsla));

Console.WriteLine("\nPaths:");
string executable = Environment.ProcessPath!;

Console.WriteLine($"Original : {executable}");
Console.WriteLine();

Console.WriteLine(PathFormatter.Format(executable, 120));
Console.WriteLine(PathFormatter.Format(executable, 80));
Console.WriteLine(PathFormatter.Format(executable, 40));
Console.WriteLine(PathFormatter.Format(executable, 20));
Console.WriteLine(PathFormatter.Format(executable, options: PathFormatOptions.FileNameOnly));
Console.WriteLine(PathFormatter.Format(executable, options: PathFormatOptions.DirectoryOnly | PathFormatOptions.NormalizeSeparators));
Console.WriteLine(PathFormatter.Format(executable, 60,
                                        PathFormatOptions.NormalizeSeparators |
                                        PathFormatOptions.TruncateFromMiddle |
                                        PathFormatOptions.IncludeRoot));

Console.WriteLine("\nNumbers:");
Console.WriteLine(NumberFormatter.Compact(1532));
Console.WriteLine(NumberFormatter.Compact(834525123));
Console.WriteLine(NumberFormatter.Human(2500000));
Console.WriteLine(NumberFormatter.FileSize(1536));
Console.WriteLine(NumberFormatter.FileSize(1536, binary: true));
Console.WriteLine(NumberFormatter.Roman(42));
Console.WriteLine(NumberFormatter.Roman(3999));

Console.WriteLine("\nDurations");
Console.WriteLine(DurationFormat.Format(TimeSpan.FromDays(3.5), "d"));                // 3
Console.WriteLine(DurationFormat.Format(TimeSpan.FromDays(3.5), "dd"));               // 03

Console.WriteLine(DurationFormat.Format(TimeSpan.FromHours(1.5), "h"));               // 1
Console.WriteLine(DurationFormat.Format(TimeSpan.FromHours(1.5), "hh"));              // 01

Console.WriteLine(DurationFormat.Format(TimeSpan.FromMinutes(90), "m"));              // 90
Console.WriteLine(DurationFormat.Format(TimeSpan.FromMinutes(90), "mm"));             // 90

Console.WriteLine(DurationFormat.Format(TimeSpan.FromSeconds(1.2345678), "s.f"));     // 1.2
Console.WriteLine(DurationFormat.Format(TimeSpan.FromSeconds(1.2345678), "s.fff"));   // 1.234
Console.WriteLine(DurationFormat.Format(TimeSpan.FromSeconds(1.2000000), "s.FFF"));   // 1.2

Console.WriteLine(DurationFormat.Format(TimeSpan.FromMilliseconds(123.456), "ms"));       // 123
Console.WriteLine(DurationFormat.Format(TimeSpan.FromMilliseconds(123.456), "ms.fff"));   // 123.456

Console.WriteLine(DurationFormat.Format(TimeSpan.FromTicks(12345), "us"));            // 1234
Console.WriteLine(DurationFormat.Format(TimeSpan.FromTicks(12345), "us.f"));          // 1234.5

Console.WriteLine(DurationFormat.Format(TimeSpan.FromTicks(123), "ns"));              // 12300

Console.WriteLine(DurationFormat.Format(TimeSpan.FromHours(1.5), "h+"));              // 1.5h
Console.WriteLine(DurationFormat.Format(TimeSpan.FromMinutes(91), "m+"));             // 90m
Console.WriteLine(DurationFormat.Format(TimeSpan.FromMilliseconds(25), "ms+"));       // 25ms

Console.WriteLine(DurationFormat.Format(TimeSpan.FromHours(1), "h++"));               // 1 hour
Console.WriteLine(DurationFormat.Format(TimeSpan.FromHours(2), "h++"));               // 2 hours
Console.WriteLine(DurationFormat.Format(TimeSpan.FromMilliseconds(1), "ms++"));       // 1 millisecond
Console.WriteLine(DurationFormat.Format(TimeSpan.FromMilliseconds(2), "ms++"));       // 2 milliseconds

Console.WriteLine(DurationFormat.Format(TimeSpan.FromMinutes(30), "[h++] [m++]"));     // 30 minutes
Console.WriteLine(DurationFormat.Format(TimeSpan.FromHours(2), "[h++] [m++]"));        // 2 hours
Console.WriteLine(DurationFormat.Format(TimeSpan.Zero, "[h++] [m++] [s++]"));           // ""

Console.WriteLine(DurationFormat.Format(
    TimeSpan.FromSeconds(1.234),
    "s.FFF+"));                                                // 1.234s

Console.WriteLine(DurationFormat.Format(
    TimeSpan.FromHours(1.5),
    "h++m+"));                                                 // 1.5 hours90m

Console.WriteLine(DurationFormat.Format(
    TimeSpan.FromDays(2.75),
    "d.fff++"));                                               // 2.750 days


TimeSpan value = TimeSpan.FromHours(1.5);

Console.WriteLine(DurationFormat.Format(value, "h++")); // 1.5 hours
Console.WriteLine(DurationFormat.Format(value, "m++")); // 90 minutes
Console.WriteLine(DurationFormat.Format(value, "s++")); // 5400 seconds

Console.WriteLine("\nDates:");

DateTime now = new(2026, 10, 5, 12, 0, 0);

Console.WriteLine("=== Relative ===");
Console.WriteLine(DateFormatter.Format(now.AddSeconds(-30), "relative[no-calendar]", now));
Console.WriteLine(DateFormatter.Format(now.AddMinutes(-5), "relative[no-calendar]", now));
Console.WriteLine(DateFormatter.Format(now.AddHours(2), "relative[no-calendar]", now));
Console.WriteLine(DateFormatter.Format(now.AddDays(3), "relative[no-calendar]", now));

Console.WriteLine();
Console.WriteLine("=== Relative (Short) ===");
Console.WriteLine(DateFormatter.Format(now.AddSeconds(-30), "relative[short, no-calendar]", now));
Console.WriteLine(DateFormatter.Format(now.AddMinutes(-5), "relative[short, no-calendar]", now));
Console.WriteLine(DateFormatter.Format(now.AddHours(2), "relative[short, no-calendar]", now));
Console.WriteLine(DateFormatter.Format(now.AddDays(3), "relative[short, no-calendar]", now));

Console.WriteLine();
Console.WriteLine("=== Calendar Aware ===");
Console.WriteLine(DateFormatter.Format(now.AddDays(-1), "relative", now));
Console.WriteLine(DateFormatter.Format(now, "relative", now));
Console.WriteLine(DateFormatter.Format(now.AddDays(1), "relative", now));

Console.WriteLine();
Console.WriteLine("=== No Calendar ===");
Console.WriteLine(DateFormatter.Format(now.AddDays(-1), "relative[no-calendar]", now));
Console.WriteLine(DateFormatter.Format(now, "relative[no-calendar]", now));
Console.WriteLine(DateFormatter.Format(now.AddDays(1), "relative[no-calendar]", now));

Console.WriteLine();
Console.WriteLine("=== Absolute Date Formats ===");
Console.WriteLine(DateFormatter.Format(now, "date[yyyy-MM-dd]", now));
Console.WriteLine(DateFormatter.Format(now, "date[dddd, dd MMMM yyyy]", now));

Console.WriteLine();
Console.WriteLine("=== Absolute Time Formats ===");
Console.WriteLine(DateFormatter.Format(now, "time[HH:mm:ss]", now));
Console.WriteLine(DateFormatter.Format(now, "time[h:mm tt]", now));

Console.WriteLine();
Console.WriteLine("=== Absolute DateTime Formats ===");
Console.WriteLine(DateFormatter.Format(now, "datetime[yyyy-MM-dd HH:mm:ss]", now));
Console.WriteLine(DateFormatter.Format(now, "datetime[dd MMM yyyy HH:mm]", now));

Console.WriteLine();
Console.WriteLine("=== Duration Conditions ===");
Console.WriteLine(DateFormatter.Format(
    now.AddMinutes(5),
    "<1h?relative:datetime[yyyy-MM-dd HH:mm]",
    now));

Console.WriteLine(DateFormatter.Format(
    now.AddHours(5),
    "<1h?relative:datetime[yyyy-MM-dd HH:mm]",
    now));

Console.WriteLine(DateFormatter.Format(
    now.AddDays(3),
    "<7d?relative:date[yyyy-MM-dd]",
    now));

Console.WriteLine(DateFormatter.Format(
    now.AddDays(30),
    "<7d?relative:date[yyyy-MM-dd]",
    now));

Console.WriteLine();
Console.WriteLine("=== Keyword Conditions ===");
Console.WriteLine(DateFormatter.Format(
    now.AddHours(-1),
    "past?relative:datetime[yyyy-MM-dd HH:mm]",
    now));

Console.WriteLine(DateFormatter.Format(
    now.AddHours(1),
    "future?relative:datetime[yyyy-MM-dd HH:mm]",
    now));

Console.WriteLine(DateFormatter.Format(
    now,
    "today?relative:date[yyyy-MM-dd]",
    now));

Console.WriteLine(DateFormatter.Format(
    now.AddDays(1),
    "tomorrow?relative:date[yyyy-MM-dd]",
    now));

Console.WriteLine(DateFormatter.Format(
    now.AddDays(-1),
    "yesterday?relative:date[yyyy-MM-dd]",
    now));

Console.WriteLine();
Console.WriteLine("=== Nested Conditions ===");
Console.WriteLine(DateFormatter.Format(
    now.AddSeconds(20),
    "<1m?relative:<1h?relative[short]:datetime[yyyy-MM-ddd HH:mm]",
    now));

Console.WriteLine(DateFormatter.Format(
    now.AddMinutes(20),
    "<1m?relative:<1h?relative[short]:datetime[yyyy-MM-dd HH:mm]",
    now));

Console.WriteLine(DateFormatter.Format(
    now.AddDays(2),
    "<1m?relative:<1h?relative[short]:datetime[yyyy-MM-dd HH:mm]",
    now));
