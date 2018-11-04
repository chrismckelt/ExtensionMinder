using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace ExtensionMinder
{
    /// <summary>
    ///     This class contain extension functions for string objects
    /// </summary>
    public static class StringExtension
    {

        public static string SurroundWithDoubleQuotes(this string text)
        {
            return SurroundWith(text, "\"");
        }

        public static string SurroundWith(this string text, string ends)
        {
            return ends + text + ends;
        }

        public static IEnumerable<string> SplitSentenceIntoWords(this string sentence)
        {
            var punctuation = sentence.Where(char.IsPunctuation).Distinct().ToArray();
            var words = sentence.Split().Select(x => x.Trim(punctuation));
            return words;
        }

        public static string Escape(this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        public static string Unescape(this string s)
        {
            return HttpUtility.HtmlDecode(s);
        }

        /// <summary>
        ///     Checks string object's value to array of string values
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stringValues"></param>
        /// <returns></returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (var otherValue in stringValues)
                if (string.CompareOrdinal(value, otherValue) == 0)
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
            return string.Format(value, arg0);
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
            return string.Format(value, args);
        }


        public static string Base64Encode(this string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText)) return null;
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            if (string.IsNullOrWhiteSpace(base64EncodedData)) return null;
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool IsValidRegex(this string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;

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
            if (!string.IsNullOrEmpty(str))
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
        ///     Use the current thread's culture info for conversion
        /// </summary>
        public static string ToTitleCase(this string str)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        ///     Overload which uses the culture info with the specified name
        /// </summary>
        public static string ToTitleCase(this string str, string cultureInfoName)
        {
            var cultureInfo = new CultureInfo(cultureInfoName);
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        ///     Overload which uses the specified culture info
        /// </summary>
        public static string ToTitleCase(this string str, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        public static TSelf TrimAllStrings<TSelf>(this TSelf obj)
        {
            if (obj == null)
                return default(TSelf);

            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.FlattenHierarchy;

            foreach (var p in obj.GetType().GetProperties(flags))
            {
                var currentNodeType = p.PropertyType;
                if (currentNodeType == typeof(string))
                {
                    var currentValue = (string) p.GetValue(obj, null);
                    if (currentValue != null) p.SetValue(obj, currentValue.Trim(), null);
                }
                // see http://stackoverflow.com/questions/4444908/detecting-native-objects-with-reflection
                else if (currentNodeType != typeof(object) && Type.GetTypeCode(currentNodeType) == TypeCode.Object)
                {
                    if (p.GetIndexParameters().Length == 0)
                        p.GetValue(obj, null).TrimAllStrings();
                    else
                        p.GetValue(obj, new object[] {0}).TrimAllStrings();
                }
            }

            return obj;
        }

        public static string Clean(this string input, string regex = "[^a-z^0-9^ ^-^_]", string replaceWith = "")
        {
            if (string.IsNullOrEmpty(input)) return input;

            string text = Regex.Replace(input, regex, replaceWith, RegexOptions.IgnoreCase).Trim();
            text = Regex.Replace(text, @"\s+", " ", RegexOptions.Multiline);
            return text;
        }

        public static string ToAlphaNumericOnly(this string input, string replaceWith = "")
        {
            var rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(input, replaceWith).Trim();
        }

        public static string ToAlphaOnly(this string input, string replaceWith = "")
        {
            var rgx = new Regex("[^a-zA-Z]");
            return rgx.Replace(input, replaceWith).Trim();
        }

        public static string ToNumericOnly(this string input, string replaceWith = "")
        {
            var rgx = new Regex("[^0-9]");
            return rgx.Replace(input, replaceWith).Trim();
        }

        public static string ToLowerInvariantWithOutSpaces(this string s)
        {
            return s.ToLowerInvariant().Replace(" ", string.Empty).Trim();
        }

        public static string TrimToLength(this string s, int length)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;

            if (s.Length > length)
                return s.Substring(0, length);

            return s;
        }

        public static string MakeValidXml(this string s)
        {
            return Regex.Replace(s, @"(?<=\<\w+)[#\{\}\(\)\&](?=\>)|(?<=\</\w+)[#\{\}\(\)\&](?=\>)", "-");
        }

        public static string MakeValidUrl(this string s)
        {
            s = Regex.Replace(s, @"[^a-z0-9\s-]", ""); // Remove all non valid chars          
            s = Regex.Replace(s, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            s = Regex.Replace(s, @"\s", "-"); // //Replace spaces by dashes
            return s;
        }

        public static string ToDefaultString(this string s, string defaultText)
        {
            if (string.IsNullOrWhiteSpace(s))
                return defaultText.Trim();

            return s.Trim();
        }

        public static string RemoveJunkWordsFromNumber(this string s)
        {
            s = s.Replace("years", string.Empty);
            s = s.Replace("year", string.Empty);
            s = s.Replace("%", string.Empty);
            s = s.Replace("$", string.Empty);
            s = s.Replace("-", string.Empty);
            return s;
        }

        public static string MakeValidFileName(this string s)
        {
            return Path.GetInvalidFileNameChars().Aggregate(s, (current, c) => current.Replace(c, '_'));
        }

        public static string ToSentenceCase(this string str)
        {
            if (str.Length == 0) return str;

            var lowerCase = str.ToLower();

            // matches the first sentence of a string, as well as subsequent sentences
            var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);

            // MatchEvaluator delegate defines replacement of sentence starts to uppercase
            var result = r.Replace(lowerCase, s => s.Value.ToUpper());

            return result;
        }

        /// <summary>
        ///     convert text to Pascal Case
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string str)
        {
            //if nothing is proivided throw a null argument exception
            if (str == null) throw new ArgumentNullException(nameof(str), "Null text cannot be converted!");

            if (str.Length == 0) return str;

            //split the provided string into an array of words
            var words = str.Split(' ');

            //loop through each word in the array
            for (var i = 0; i < words.Length; i++)
                //if the current word is greater than 1 character long
                if (words[i].Length > 0)
                {
                    //grab the current word
                    var word = words[i];

                    //convert the first letter in the word to uppercase
                    var firstLetter = char.ToUpper(word[0]);

                    //concantenate the uppercase letter to the rest of the word
                    words[i] = firstLetter + word.Substring(1);
                }

            //return the converted text
            return string.Join(string.Empty, words);
        }

        public static string AddSpacesToSentence(this string text)
        {
            return Regex.Replace(text, "([a-z])([A-Z])", "$1 $2");
        }

        /// <summary>
        ///     Gibberish, alternatively jibberish, jibber-jabber, or gobbledygook, is language that is (or appears to be)
        ///     nonsense.
        ///     It may include speech sounds that are not actual words,[1] or language games and specialized jargon that seems
        ///     nonsensical to outsiders.
        ///     [2] Gibberish should not be confused with literary nonsense such as that used in the poem "Jabberwocky" by Lewis
        ///     Carroll.
        /// </summary>
        public static bool IsGibberish(this string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return true;

            try
            {
                var chars = word.ToCharArray();
                if (!chars.Any()) return true;

                var numbers = chars.Count(char.IsDigit);
                var letters = chars.Count(char.IsLetter);

                if (letters == 0) return true;

                return numbers > letters;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return true;
            }
        }
    }
}