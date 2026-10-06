namespace WinterRose.Formatting.Paths;

public static class PathFormatter
{
    public static string Format(
        string path,
        int maxLength = 50,
        PathFormatOptions options = PathFormatOptions.Default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        if (options.HasFlag(PathFormatOptions.NormalizeSeparators))
        {
            path = path
                .Replace('\\', '/')
                .Replace("//", "/");
        }

        if (options.HasFlag(PathFormatOptions.FileNameOnly))
            return Path.GetFileName(path);

        if (options.HasFlag(PathFormatOptions.DirectoryOnly))
            return Path.GetDirectoryName(path) ?? string.Empty;

        if (maxLength <= 0 || path.Length <= maxLength)
            return path;

        if (options.HasFlag(PathFormatOptions.TruncateFromStart))
            return TruncateFromStart(path, maxLength);

        if (options.HasFlag(PathFormatOptions.TruncateLastSegment))
            return TruncateLastSegment(path, maxLength);

        return TruncateFromMiddle(
            path,
            maxLength,
            options.HasFlag(PathFormatOptions.IncludeRoot));
    }

    private static string TruncateFromStart(
        string path,
        int maxLength)
    {
        if (path.Length <= maxLength)
            return path;

        return "..." + path[^Math.Max(0, maxLength - 3)..];
    }

    private static string TruncateLastSegment(
        string path,
        int maxLength)
    {
        string normalized = path.Replace('\\', '/');

        string fileName = Path.GetFileName(normalized);

        if (fileName.Length + 4 >= maxLength)
            return TruncateFromStart(fileName, maxLength);

        string directory =
            Path.GetDirectoryName(normalized)?
            .Replace('\\', '/')
            ?? string.Empty;

        string[] segments = directory
            .Split('/', StringSplitOptions.RemoveEmptyEntries);

        List<string> keptSegments = [];

        foreach (string segment in segments)
        {
            string candidate = string.Join('/', keptSegments.Append(segment));

            string formatted =
                $"{candidate}/.../{fileName}";

            if (formatted.Length > maxLength)
                break;

            keptSegments.Add(segment);
        }

        string kept = string.Join('/', keptSegments);

        return string.IsNullOrEmpty(kept)
            ? $".../{fileName}"
            : $"{kept}/.../{fileName}";
    }

    private static string TruncateFromMiddle(
        string path,
        int maxLength,
        bool includeRoot)
    {
        string normalized = path.Replace('\\', '/');

        string root = string.Empty;

        if (includeRoot)
        {
            root = Path.GetPathRoot(path)?
                .Replace('\\', '/')
                ?? string.Empty;
        }

        string[] segments = normalized
            .Substring(root.Length)
            .Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length <= 1)
            return TruncateFromStart(normalized, maxLength);

        string fileName = segments[^1];

        List<string> leftSegments = [];

        int currentLength =
            root.Length +
            fileName.Length;

        for (int i = 0; i < segments.Length - 1; i++)
        {
            string segment = segments[i];

            int requiredLength =
                currentLength +
                segment.Length +
                1 + // slash before segment
                4 + // "/..."
                1 + // slash after "..."
                fileName.Length;

            if (requiredLength > maxLength)
                break;

            leftSegments.Add(segment);
            currentLength += segment.Length + 1;
        }

        string left = string.Join('/', leftSegments);

        return string.IsNullOrEmpty(left)
            ? $"{root}.../{fileName}"
            : $"{root}{left}/.../{fileName}";
    }

}
