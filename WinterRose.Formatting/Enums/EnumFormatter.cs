using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterRose.Formatting.Enums;

/// <summary>
/// Provides methods to format enum values for user facing UIs
/// </summary>
using System.ComponentModel;
using System.Reflection;
using System.Text;
using WinterRose.Formatting.Collections;

/// <summary>
/// Provides methods to make an enum value readable for a human facing UI
/// </summary>
public static class EnumFormatter
{
    extension<T>(T e) where T : struct, Enum
    {
        /// <summary>
        /// Makes the enum value readable for user facing UIs. <br/>
        /// Supports standard enums and flag enums. Flag enums have their zero value always hidden unless theyre the only value
        /// </summary>
        public string Humanize()
        {
            Type enumType = typeof(T);

            // [Flags]
            if (enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                ulong numericValue = Convert.ToUInt64(e);

                if (numericValue == 0)
                    return GetDisplayText<T>(e.ToString());

                List<string> parts = [];

                foreach (T value in Enum.GetValues<T>())
                {
                    ulong flagValue = Convert.ToUInt64(value);

                    if (flagValue == 0)
                        continue;

                    if ((numericValue & flagValue) == flagValue)
                    {
                        parts.Add(GetDisplayText<T>(value.ToString()));
                    }
                }

                return parts.ToDelimitedText();
            }

            return GetDisplayText<T>(e.ToString());
        }
    }
    private static string GetDisplayText<T>(string enumName)
    {
        //FieldInfo? field = typeof(T).GetField(enumName);

        //if (field is not null)
        //{
        //    DisplayNameAttribute? displayName =
        //        field.GetCustomAttribute<DisplayNameAttribute>();

        //    if (!string.IsNullOrWhiteSpace(displayName?.DisplayName))
        //        return displayName.DisplayName;
        //}

        return HumanizeIdentifier(enumName);
    }

    private static string HumanizeIdentifier(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        StringBuilder builder = new(text.Length + 8);

        builder.Append(text[0]);

        for (int i = 1; i < text.Length; i++)
        {
            char current = text[i];
            char previous = text[i - 1];

            if (char.IsUpper(current) && (char.IsLower(previous) || char.IsDigit(previous)))
                builder.Append(' ');

            builder.Append(current);
        }

        return builder.ToString();
    }
}