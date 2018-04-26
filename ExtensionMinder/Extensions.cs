using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web;

namespace ExtensionMinder
{
    public static class Extensions
    {
        public static T As<T>(this object value)
        {
            return (T) value;
        }

        public static string ToLowerInvariantWithOutSpaces(this string s)
        {
            return s.ToLowerInvariant().Replace(" ", string.Empty).Trim();
        }

        public static string TrimToLength(this string s, int length)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;

            if(s.Length > length)
                return s.Substring(0, length);

            return s;
        }

        public static string MakeValidXml(this string s)
        {
            return Regex.Replace(s, (@"(?<=\<\w+)[#\{\}\(\)\&](?=\>)|(?<=\</\w+)[#\{\}\(\)\&](?=\>)"), "-");
        }

        public static string MakeValidUrl(this string s)
        {
            s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-z0-9\s-]", ""); // Remove all non valid chars          
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s", "-"); // //Replace spaces by dashes
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
            return System.IO.Path.GetInvalidFileNameChars().Aggregate(s, (current, c) => current.Replace(c, '_'));
        }

        public static string Capitalize(this string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return string.Empty;

            return word[0].ToString().ToUpper() + word.Substring(1);
        }

        public static Hashtable ConvertPropertiesAndValuesToHashtable(this object obj)
        {
            var ht = new Hashtable();

            // get all public static properties of obj type
            PropertyInfo[] propertyInfos =
                obj.GetType().GetProperties().Where(a => a.MemberType.Equals(MemberTypes.Property)).ToArray();
            // sort properties by name
            Array.Sort(propertyInfos, (propertyInfo1, propertyInfo2) => propertyInfo1.Name.CompareTo(propertyInfo2.Name));

            // write property names
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                ht.Add(propertyInfo.Name,
                       propertyInfo.GetValue(obj, BindingFlags.Public, null, null, CultureInfo.CurrentCulture));
            }

            return ht;
        }

        public static string FirstSortableProperty(this Type type)
        {
            PropertyInfo firstSortableProperty = type.GetProperties().Where(property => property.PropertyType.IsPredefinedType()).FirstOrDefault();

            if (firstSortableProperty == null)
            {
                throw new NotSupportedException("Cannot find property to sort by.");
            }

            return firstSortableProperty.Name;
        }

        internal static bool IsPredefinedType(this Type type)
        {
            return PredefinedTypes.Any(t => t == type);
        }

        public static readonly Type[] PredefinedTypes = {
            typeof(Object),
            typeof(Boolean),
            typeof(Char),
            typeof(String),
            typeof(SByte),
            typeof(Byte),
            typeof(Int16),
            typeof(UInt16),
            typeof(Int32),
            typeof(UInt32),
            typeof(Int64),
            typeof(UInt64),
            typeof(Single),
            typeof(Double),
            typeof(Decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Math),
            typeof(Convert)
        };

        public static Guid IfGuidEmptyCreateNew(this Guid guid)
        {
            if (guid == Guid.Empty)
            {
                return Guid.NewGuid();
            }

            return guid;
        }


        /// <summary>
        /// Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
        /// </summary>
        /// <param name="source">Source (UTC format)</param>
        /// <returns>Formatted date and time string</returns>
        public static string RelativeFormat(this DateTime source)
        {
            return RelativeFormat(source, string.Empty);
        }


        /// <summary>
        /// Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
        /// </summary>
        /// <param name="source">Source (UTC format)</param>
        /// <param name="convertToUserTime">A value indicating whether we should convet DateTime instance to user local time (in case relative formatting is not applied)</param>
        /// <param name="defaultFormat">Default format string (in case relative formatting is not applied)</param>
        /// <returns>Formatted date and time string</returns>
        public static string RelativeFormat(this DateTime source, string defaultFormat)
        {
            string result = string.Empty;

            //TODO localize hard-coded strings
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - source.Ticks);
            double delta = ts.TotalSeconds;

            if (delta > 0)
            {
                if (delta < 60) // 60 (seconds)
                {
                    result = ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
                }
                else if (delta < 120) //2 (minutes) * 60 (seconds)
                {
                    result = "a minute ago";
                }
                else if (delta < 2700) // 45 (minutes) * 60 (seconds)
                {
                    result = ts.Minutes + " minutes ago";
                }
                else if (delta < 5400) // 90 (minutes) * 60 (seconds)
                {
                    result = "an hour ago";
                }
                else if (delta < 86400) // 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int hours = ts.Hours;
                    if (hours == 1)
                        hours = 2;
                    result = hours + " hours ago";
                }
                else if (delta < 172800) // 48 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = "yesterday";
                }
                else if (delta < 2592000) // 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = ts.Days + " days ago";
                }
                else if (delta < 31104000) // 12 (months) * 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    result = months <= 1 ? "one month ago" : months + " months ago";
                }
                else
                {
                    int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    result = years <= 1 ? "one year ago" : years + " years ago";
                }
            }
            else
            {
                DateTime tmp1 = source;

                //default formatting
                if (!String.IsNullOrEmpty(defaultFormat))
                {
                    result = tmp1.ToString(defaultFormat);
                }
                else
                {
                    result = tmp1.ToString();
                }
            }
            return result;
        }


        public static int ToXmlBoolean(this bool value)
        {
            return value ? 1 : 0;
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

        public static string AddSpacesToSentence(this string text)
        {
            return Regex.Replace(text, "([a-z])([A-Z])", "$1 $2");
        }

        //public static MvcHtmlString ToMvcHtmlString(this string str)
        //{15
        //    return new MvcHtmlString(str);
        //}

        /// <summary>
        ///convert text to Pascal Case
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string str)
        {
            //if nothing is proivided throw a null argument exception
            if (str == null) throw new ArgumentNullException("str", "Null text cannot be converted!");

            if (str.Length == 0) return str;

            //split the provided string into an array of words
            string[] words = str.Split(' ');

            //loop through each word in the array
            for (int i = 0; i < words.Length; i++)
            {
                //if the current word is greater than 1 character long
                if (words[i].Length > 0)
                {
                    //grab the current word
                    string word = words[i];

                    //convert the first letter in the word to uppercase
                    char firstLetter = char.ToUpper(word[0]);

                    //concantenate the uppercase letter to the rest of the word
                    words[i] = firstLetter + word.Substring(1);
                }
            }

            //return the converted text
            return string.Join(string.Empty, words);
        }

        public static string ToQueryString(this NameValueCollection coll)
        {
            return string.Join("&", 
                coll.Cast<string>().Select(a => string.Format("{0}={1}",
                    HttpUtility.UrlEncode(a), 
                    HttpUtility.UrlEncode(coll[a]))));
        }

        public static PropertyInfo GetProperty<TX, TY>(this TX obj, Expression<Func<TX, TY>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            var attrType = typeof(T);
            return (T)property.GetCustomAttributes(attrType, false).FirstOrDefault();
        }

        public static bool IsSystem(this PropertyInfo property)
        {
            return (property.PropertyType.Namespace != null && property.PropertyType.Namespace.StartsWith("System"));
        }

        public static T To<T>(this object value)
        {
            return value == null ? default(T) : value.ToString().To<T>();
        }

        public static T To<T>(this string value)
        {
            if (typeof(T).IsEnum)
            {
                if (string.IsNullOrEmpty(value)) return default(T);
                return (T)Enum.Parse(typeof(T), value);
            }

            return String.IsNullOrEmpty(value) ? default(T) : (T)Convert.ChangeType(value, typeof(T));
        }

        public static string ShowIf(this string answer, Func<bool> question)
        {
            return question() ? answer : "";
        }

        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new InvalidOperationException(@"DeepClone: The type must be serializable.");
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(a, null))
            {
                return default(T);
            }

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static string GetContent(this Stream stream)
        {
            if (stream == null) throw new Exception("No stream available for the request");
            return new StreamReader(stream).ReadToEnd();
        }

        public static void ToFile(this Stream stream, string filepath)
        {
            var file = new FileInfo(filepath);

            if (file.Directory != null)
                file.Directory.Create();

            using (var fileStream = File.Create(filepath))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}


