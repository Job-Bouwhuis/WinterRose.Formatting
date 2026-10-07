using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterRose.Formatting.Collections;

/// <summary>
/// Provides extension methods to format a collection of strings
/// </summary>
public static class StringCollectionFormatter
{
    extension(IEnumerable<string> values)
    {
        /// <summary>
        /// Formats a collection of strings. <br/>
        /// Examples:
        /// <list type="bullet">
        ///     <item>a</item>
        ///     <item>a and b</item>
        ///     <item>a, b, and c</item>
        ///     <item>a, b, c, and d</item>
        ///     <item>etc</item>
        /// </list>
        /// </summary>
        /// <returns></returns>
        public string ToDelimitedText()
        {
            string[] items = values
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();

            return items.Length switch
            {
                0 => string.Empty,
                1 => items[0],
                2 => $"{items[0]} and {items[1]}",
                _ => $"{string.Join(", ", items[..^1])}, and {items[^1]}"
            };
        }
    }

}
