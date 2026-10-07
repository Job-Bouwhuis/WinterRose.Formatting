using System;
using System.Collections.Generic;
using System.Text;

namespace WinterRose.Formatting.Text;

/// <summary>
/// Provides methods for formatting and manipulating text.
/// </summary>
public static class TextFormatting
{
    /// <summary>
    /// Truncates a string to the specified number of characters.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="length">The maximum number of characters.</param>
    /// <returns>The truncated string.</returns>
    public static string Truncate(string value, int length)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        return value.Length <= length
            ? value
            : value[..length];
    }

    /// <summary>
    /// Truncates a string to the specified number of characters without cutting a word.
    /// Spaces and newlines are treated as word separators.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="length">The maximum number of characters.</param>
    /// <returns>The truncated string.</returns>
    public static string TruncateWords(string value, int length)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        if (value.Length <= length)
            return value;

        int position = length;

        while (position > 0 && !char.IsWhiteSpace(value[position - 1]))
            position--;

        return value[..position].TrimEnd();
    }

    /// <summary>
    /// Normalizes consecutive whitespace characters into a single space.
    /// </summary>
    /// <param name="value">The string to normalize.</param>
    /// <returns>The normalized string.</returns>
    public static string NormalizeWhitespace(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        StringBuilder result = new(value.Length);
        bool whitespace = false;

        foreach (char character in value)
        {
            if (char.IsWhiteSpace(character))
            {
                whitespace = true;
                continue;
            }

            if (whitespace && result.Length > 0)
                result.Append(' ');

            result.Append(character);
            whitespace = false;
        }

        return result.ToString();
    }

    /// <summary>
    /// Normalizes all newline representations to <see cref="Environment.NewLine"/>.
    /// </summary>
    /// <param name="value">The string to normalize.</param>
    /// <returns>The string with normalized newline characters.</returns>
    public static string NormalizeNewlines(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return value
            .Replace("\r\n", "\n")
            .Replace('\r', '\n')
            .Replace("\n", Environment.NewLine);
    }
}
