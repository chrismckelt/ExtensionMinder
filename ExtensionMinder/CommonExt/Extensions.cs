using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace ExtensionMinder.CommonExt
{
    public static class Extensions
    {
        public static T Dump<T>(this T obj, string message, int depth = 4, int indentSize = 2, char indentChar = ' ')
        {
            Console.WriteLine(message);
            var dump = ObjectDumper.Dump(obj,depth,indentSize, indentChar);
            Console.Write(dump);
            return obj;
        }

        public static T As<T>(this object value)
        {
            return (T) value;
        }

        public static Guid ToNewGuidIfEmpty(this Guid guid)
        {
            return guid == Guid.Empty ? Guid.NewGuid() : guid;
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
                return (T) Enum.Parse(typeof(T), value);
            }

            return string.IsNullOrEmpty(value) ? default(T) : (T) Convert.ChangeType(value, typeof(T));
        }

        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            if (!typeof(T).IsSerializable)
                throw new InvalidOperationException(@"DeepClone: The type must be serializable.");

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(a, null)) return default(T);

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T) formatter.Deserialize(stream);
            }
        }
    }
}