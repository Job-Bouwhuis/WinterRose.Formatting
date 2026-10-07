using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WinterRose.Formatting;
using WinterRose.Formatting.Colors;
using WinterRose.Formatting.Currency;
using WinterRose.Formatting.Enums;
using WinterRose.Formatting.Paths;
using WinterRose.Formatting.TimeFormats;

Console.WriteLine(CurrencyConverter.Convert(100, Currency.Euro, Currency.JapaneseYen));
Console.WriteLine(CurrencyConverter.Convert(100, Currency.Euro, Currency.UnitedStatesDollar));
Console.WriteLine(CurrencyConverter.Convert(100, Currency.Euro, Currency.CzechKoruna));
Console.WriteLine(CurrencyConverter.Convert(100, Currency.Euro, Currency.DanishKrone));
Console.WriteLine(CurrencyConverter.Convert(100, Currency.Euro, Currency.CanadianDollar));
Console.WriteLine(CurrencyConverter.Convert(100, Currency.Euro, Currency.ChineseYuan));

DateTime processStarted = DateTime.Now.AddDays(-1);
Console.WriteLine(DateFormatter.Format(processStarted, "relative")); // yesterday

// Today is Tuesday. Spooky day to you! (its currently tuesday 6th october at time of writing)
Console.WriteLine(DateFormatter.Format(DateTime.Now, 
    """
    Today is @weekday. ;month {
        12: "Merry Christmas!",
        10: "Spooky day to you!",
        default: "Have a nice day."
    }
    """));

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromHours(1.5), "m+ h+")); // 90m 0h
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromHours(1.5), "h+ m+")); // 1h 30m


Console.WriteLine("Color:");
Console.WriteLine(ColorFormatter.Format(255, 128, 0, ColorFormatKind.Hex)); // #FF8000
Console.WriteLine(ColorFormatter.Format(255, 128, 0, ColorFormatKind.Rgb)); // rgb(255, 128, 0)
Console.WriteLine(ColorFormatter.Format(255, 128, 0, 128, ColorFormatKind.Rgba)); // rgba(255, 128, 0, 0.5)
Console.WriteLine(ColorFormatter.Format(255, 128, 0, ColorFormatKind.Hsl)); // hsl(30, 100%, 50%)
Console.WriteLine(ColorFormatter.Format(255, 128, 0, 128, ColorFormatKind.Hsla)); // hsla(30, 100%, 50%, 0.5)

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

/*
 output from above code will look like this (on Windows):
Original : D:\GitRepositories\Personal\WinterRose.Formatting\WinterRose.Formatting.Tests\bin\Debug\net10.0\WinterRose.Formatting.Tests.exe

D:/GitRepositories/Personal/WinterRose.Formatting/.../WinterRose.Formatting.Tests.exe
D:/.../WinterRose.Formatting.Tests.exe
.../WinterRose.Formatting.Tests.exe
.../WinterRose.Formatting.Tests.exe
WinterRose.Formatting.Tests.exe
D:\GitRepositories\Personal\WinterRose.Formatting\WinterRose.Formatting.Tests\bin\Debug\net10.0
D:/.../WinterRose.Formatting.Tests.exe
 */

Console.WriteLine("\nNumbers:");
Console.WriteLine(NumberFormatter.Compact(1532)); // 1.53K
Console.WriteLine(NumberFormatter.Compact(834525123)); // 834.53M
Console.WriteLine(NumberFormatter.Human(2500000)); // 2.5 million
Console.WriteLine(NumberFormatter.FileSize(1536)); // 1.54 KB
Console.WriteLine(NumberFormatter.FileSize(1536, binary: true)); // 1.50 KiB
Console.WriteLine(NumberFormatter.Roman(42)); // XLII
Console.WriteLine(NumberFormatter.Roman(3999)); // MMMCMXCIX

Console.WriteLine("\nDurations");
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromDays(3.5), "d"));                // 3
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromDays(3.5), "dd"));               // 03

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromHours(1.5), "h"));               // 1
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromHours(1.5), "hh"));              // 01

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromMinutes(90), "m"));              // 90
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromMinutes(90), "mm"));             // 90

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromSeconds(1.2345678), "s.f"));     // 1.2
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromSeconds(1.2345678), "s.fff"));   // 1.234
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromSeconds(1.2000000), "s.FFF"));   // 1.2

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromMilliseconds(123.456), "ms"));       // 123
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromMilliseconds(123.456), "ms.fff"));   // 123.456

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromTicks(12345), "us"));            // 1234
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromTicks(12345), "us.f"));          // 1234.5

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromTicks(123), "ns"));              // 12300

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromHours(1.5), "h+"));              // 1.5h
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromMinutes(91), "m+"));             // 90m
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromMilliseconds(25), "ms+"));       // 25ms

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromHours(1), "h++"));               // 1 hour
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromHours(2), "h++"));               // 2 hours
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromMilliseconds(1), "ms++"));       // 1 millisecond
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromMilliseconds(2), "ms++"));       // 2 milliseconds

Console.WriteLine(DurationFormatter.Format(TimeSpan.FromMinutes(30), "[h++] [m++]"));     // 30 minutes
Console.WriteLine(DurationFormatter.Format(TimeSpan.FromHours(2), "[h++] [m++]"));        // 2 hours
Console.WriteLine(DurationFormatter.Format(TimeSpan.Zero, "[h++] [m++] [s++]"));           // ""

Console.WriteLine(DurationFormatter.Format(
    TimeSpan.FromSeconds(1.234),
    "s.FFF+"));                                                // 1.234s

Console.WriteLine(DurationFormatter.Format(
    TimeSpan.FromHours(1.5),
    "h++m+"));                                                 // 1.5 hours90m

Console.WriteLine(DurationFormatter.Format(
    TimeSpan.FromDays(2.75),
    "d.fff++"));                                               // 2.750 days


TimeSpan value = TimeSpan.FromHours(1.5);

Console.WriteLine(DurationFormatter.Format(value, "h++")); // 1.5 hours
Console.WriteLine(DurationFormatter.Format(value, "m++")); // 90 minutes
Console.WriteLine(DurationFormatter.Format(value, "s++")); // 5400 seconds

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


/*
 output from above date formatting code will look like this (assuming now is 2026-10-05 12:00:00):
=== Relative ===
30 seconds ago
5 minutes ago
in 2 hours
in 3 days

=== Relative (Short) ===
30s ago
5m ago
in 2h
in 3d

=== Calendar Aware ===
yesterday
today
tomorrow

=== No Calendar ===
1 day ago
just now
in 1 day

=== Absolute Date Formats ===
2026-10-05
maandag, 05 oktober 2026

=== Absolute Time Formats ===
12:00:00
12:00

=== Absolute DateTime Formats ===
2026-10-05 12:00:00
05 okt 2026 12:00

=== Duration Conditions ===
today
2026-10-05 17:00
in 3 days
2026-11-04

=== Keyword Conditions ===
today
today
today
tomorrow
yesterday

=== Nested Conditions ===
today
today
2026-10-07 12:00
*/


Console.WriteLine(SupplierStatus.None.Humanize());
Console.WriteLine(SupplierStatus.WaitingOnSupplier.Humanize());
Console.WriteLine(SupplierStatus.QuoteReceived.Humanize());
Console.WriteLine(SupplierStatus.AwaitingApproval.Humanize());
Console.WriteLine((SupplierStatus.AwaitingApproval | SupplierStatus.None |SupplierStatus.QuoteReceived).Humanize());

[Flags]
public enum SupplierStatus
{
    None = 0,

    WaitingOnSupplier = 1,

    QuoteReceived = 2,

    AwaitingApproval = 4
}