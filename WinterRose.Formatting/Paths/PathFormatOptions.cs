namespace WinterRose.Formatting.Paths;

[Flags]
public enum PathFormatOptions
{
    None = 0,
    Default = NormalizeSeparators | TruncateFromMiddle,

    // Path separator handling.
    NormalizeSeparators = 1 << 0,

    // Output style.
    FileNameOnly = 1 << 1,
    DirectoryOnly = 1 << 2,
    IncludeRoot = 1 << 3,

    // Truncation behavior.
    TruncateFromStart = 1 << 4,
    TruncateFromMiddle = 1 << 5,
    TruncateLastSegment = 1 << 6,
}
