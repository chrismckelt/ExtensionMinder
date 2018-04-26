using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace ExtensionMinder
{
    public static class EnumExtensions
    {
        public static T ParseAsEnumByDescriptionAttribute<T>(this string description) // where T : enum
        {
            if (String.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException(description, @"Cannot parse an empty description");
            }

            Type enumType = typeof (T);
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException(String.Format("Invalid Enum type{0}", typeof (T)));
            }

            foreach (T item in Enum.GetValues(typeof (T)))
            {
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])
                        item.GetType()
                            .GetField(item.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false);
                if (attributes.Length > 0 && attributes[0].Description.ToUpper() == description.ToUpper())
                {
                    return item;
                }
            }
            throw new InvalidOperationException(String.Format("Couldn't find enum of type {0} with attribute of '{1}'",
                typeof (T), description));
        }


        public static string GetXmlEnumAttribute(this Enum enumerationValue)
        {
            XmlEnumAttribute[] attributes =
                (XmlEnumAttribute[])
                    enumerationValue.GetType()
                        .GetField(enumerationValue.ToString())
                        .GetCustomAttributes(typeof (XmlEnumAttribute), false);
            return attributes.Length > 0 ? attributes[0].Name : enumerationValue.ToString();
        }

        public static T GetEnumAttribute<T>(this Enum enumerationValue) where T : Attribute
        {
            return
                ((T[])
                    enumerationValue.GetType()
                        .GetField(enumerationValue.ToString())
                        .GetCustomAttributes(typeof (T), false))
                    .FirstOrDefault();
        }

        /// <summary>
        /// Gets all items for an enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAllItems<T>(this Enum value)
        {
            return from object item in Enum.GetValues(typeof (T)) select (T) item;
        }

        /// <summary>
        /// Gets all items for an enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAllItems<T>() where T : struct
        {
            return Enum.GetValues(typeof (T)).Cast<T>();
        }

        /// <summary>
        /// Gets all combined items from an enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <example>
        /// Displays ValueA and ValueB.
        /// <code>
        /// EnumExample dummy = EnumExample.Combi;
        /// foreach (var item in dummy.GetAllSelectedItems<EnumExample>())
        /// {
        ///    Console.WriteLine(item);
        /// }
        /// </code>
        /// </example>
        public static IEnumerable<T> GetAllSelectedItems<T>(this Enum value)
        {
            int valueAsInt = Convert.ToInt32(value, CultureInfo.InvariantCulture);

            return from object item in Enum.GetValues(typeof (T))
                let itemAsInt = Convert.ToInt32(item, CultureInfo.InvariantCulture)
                where itemAsInt == (valueAsInt & itemAsInt)
                select (T) item;
        }

        /// <summary>
        /// Determines whether the enum value contains a specific value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        ///     <c>true</c> if value contains the specified value; otherwise, <c>false</c>.
        /// </returns>
        /// <example>
        /// <code>
        /// EnumExample dummy = EnumExample.Combi;
        /// if (dummy.Contains<EnumExample>(EnumExample.ValueA))
        /// {
        ///     Console.WriteLine("dummy contains EnumExample.ValueA");
        /// }
        /// </code>
        /// </example>
        public static bool Contains<T>(this Enum value, T request)
        {
            int valueAsInt = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            int requestAsInt = Convert.ToInt32(request, CultureInfo.InvariantCulture);

            if (requestAsInt == (valueAsInt & requestAsInt))
            {
                return true;
            }

            return false;
        }

        public static T? TryParseEnum<T>(this string enumerationString) where T : struct
        {
            if (GetAllItems<T>().Any(e => e.ToString() == enumerationString))
            {
                return enumerationString.ToEnum<T>();
            }

            return null;
        }

        public static T ToEnum<T>(this string enumerationString)
        {
            if (String.IsNullOrWhiteSpace(enumerationString))
                return default(T);

            return (T) Enum.Parse(typeof (T), enumerationString);
        }

        public static string GetDescription(this Enum enumerationValue)
        {
            if (enumerationValue == null)
                return null;
            var attributes =
                (DescriptionAttribute[])
                    enumerationValue.GetType()
                        .GetField(enumerationValue.ToString())
                        .GetCustomAttributes(typeof (DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : enumerationValue.ToString();
        }
    }
}