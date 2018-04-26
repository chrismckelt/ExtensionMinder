using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ExtensionMinder
{
    /// <summary>
    ///     This class contain extension functions for string objects
    /// </summary>
    public static class StringExtension
    {
        public static string Escape(this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        public static string Unescape(this string s)
        {
            return HttpUtility.HtmlDecode(s);
        }

       /// <summary>
        ///  Checks string object's value to array of string values
       /// </summary>
       /// <param name="value"></param>
       /// <param name="stringValues"></param>
       /// <returns></returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
                if (String.CompareOrdinal(value, otherValue) == 0)
                    return true;

            return false;
        }

        /// <summary>
        ///     Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }

        /// <summary>
        ///     Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }

        /// <summary>
        ///     Replaces the format item in a specified System.String with the text equivalent
        ///     of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="arg0">An System.Object to format</param>
        /// <returns>
        ///     A copy of format in which the first format item has been replaced by the
        ///     System.String equivalent of arg0
        /// </returns>
        public static string Format(this string value, object arg0)
        {
            return String.Format(value, arg0);
        }

        /// <summary>
        ///     Replaces the format item in a specified System.String with the text equivalent
        ///     of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns>
        ///     A copy of format in which the format items have been replaced by the System.String
        ///     equivalent of the corresponding instances of System.Object in args.
        /// </returns>
        public static string Format(this string value, params object[] args)
        {
            return String.Format(value, args);
        }


        public static string Base64Encode(this string plainText)
        {
            if (String.IsNullOrWhiteSpace(plainText)) return null;
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            if (String.IsNullOrWhiteSpace(base64EncodedData)) return null;
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool IsValidRegex(this string pattern)
        {
            if (String.IsNullOrEmpty(pattern)) return false;

            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }


        public static string ToCamelCase(this string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                if (str.Length < 2) return str.ToLowerInvariant();

                return str.Substring(0, 1).ToLowerInvariant() + str.Substring(1);
            }

            return str;
        }

        public static bool EndsWithWhitespace(string q)
        {
            return Regex.IsMatch(q, "^.*\\s$");
        }

        /// <summary>
        /// Use the current thread's culture info for conversion
        /// </summary>
        public static string ToTitleCase(this string str)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the culture info with the specified name
        /// </summary>
        public static string ToTitleCase(this string str, string cultureInfoName)
        {
            var cultureInfo = new CultureInfo(cultureInfoName);
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the specified culture info
        /// </summary>
        public static string ToTitleCase(this string str, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
