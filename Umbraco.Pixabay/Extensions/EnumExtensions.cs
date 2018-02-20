using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Umbraco.Pixabay.Extensions
{
    internal static class EnumExtensions
    {
        public enum StringCase
        {
            /// <summary>
            /// The default capitalization
            /// </summary>
            Default,
            /// <summary>
            /// Lower Case, ex. i like widgets.
            /// </summary>
            [Description("Lower Case")]
            Lower,
            /// <summary>
            /// Upper Case, ex. I LIKE WIDGETS.
            /// </summary>
            [Description("Upper Case")]
            Upper,
            /// <summary>
            /// Lower Camelcase, ex: iLikeWidgets.
            /// </summary>
            [Description("Lower Camelcase")]
            LowerCamel,
            /// <summary>
            /// Upper Camelcase, ex: ILikeWidgets.
            /// </summary>
            [Description("Upper Camelcase")]
            UpperCamel
        }

        /// <summary>
        /// Get the value of an enum as a string.
        /// </summary>
        /// <param name="val"> The enum to convert to a <see cref="string"/>. </param>
        /// <param name="case"> A <see cref="StringCase"/> indicating which case to return.  Valid enumerations are StringCase.Lower and StringCase.Upper. </param>
        /// <exception cref="ArgumentNullException"> If the enum is null. </exception>
        /// <returns></returns>
        public static string GetName(this Enum val, StringCase @case = StringCase.Default)
        {
            if (val == null) throw new ArgumentNullException(nameof(val));

            var name = Enum.GetName(val.GetType(), val);
            if (name == null) return null;

            switch (@case)
            {
                case StringCase.Lower:
                    return name.ToLower();
                case StringCase.Upper:
                    return name.ToUpper();
                default:
                    return name;
            }
        }

        public static string GetDescription<T>(this T val)
        {
            if (val == null) throw new ArgumentNullException(nameof(val));

            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidEnumArgumentException();
            var castVal = val as Enum;

            var fields = type.GetField(castVal.GetName());

            // first try and pull out the EnumMemberAttribute, common when using a JsonSerializer
            var jsonAttribute = fields.GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault() as EnumMemberAttribute;
            if (jsonAttribute != null) return jsonAttribute.Value;

            // If that doesn't work, do the regular description, that still fails, just return a pretty ToString().
            var attribute = fields.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return attribute == null ? castVal.GetName() : attribute.Description;
        }

        public static T? ToEnum<T>(this string @string, bool ignoreCase = false) where T : struct
        {
            T val;
            var result = Enum.TryParse(@string, ignoreCase, out val);
            return result ? new T?(val) : null;
        }
    }
}
