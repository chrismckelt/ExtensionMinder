using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace ExtensionMinder
{
    public static class SerializerExtensions
    {
        public static string SerializeToBinary<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, source);
                stream.Flush();
                stream.Position = 0;
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static T DeserializeFromBinary<T>(this string str)
        {
            byte[] bytes = Convert.FromBase64String(str);

            using (var stream = new MemoryStream(bytes))
            {
                return (T)new BinaryFormatter().Deserialize(stream);
            }
        }

        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string SerializeWithDataContractSerializer(this object obj)
        {
            var serialXml = new StringBuilder();
            var dcSerializer = new DataContractSerializer(obj.GetType());
            using (var xWriter = XmlWriter.Create(serialXml))
            {
                dcSerializer.WriteObject(xWriter, obj);
                xWriter.Flush();
                return serialXml.ToString();
            }

        }

        public static string SerializeWithDataContractSerializer(this object obj, IList<Type> knownTypes)
        {
            var serialXml = new StringBuilder();
            var dcSerializer = new DataContractSerializer(obj.GetType(), knownTypes);
            using (var xWriter = XmlWriter.Create(serialXml))
            {
                dcSerializer.WriteObject(xWriter, obj);
                xWriter.Flush();
                return serialXml.ToString();
            }

        }

        public static T Deserialize<T>(this XmlDocument xmlDocument)
        {
            var ser = new DataContractSerializer(typeof(T));
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(stringWriter))
            {
                xmlDocument.WriteTo(xmlWriter);
                var stream = new MemoryStream(Encoding.GetEncoding("UTF-16").GetBytes(stringWriter.ToString()))
                {
                    Position = 0
                };
                var reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
                return (T)ser.ReadObject(reader, true);
            }
        }


        public static T Deserialize<T>(this byte[] buffer)
        {

            var formatter = new BinaryFormatter();
            var ms = new MemoryStream(buffer);
            return (T)formatter.Deserialize(ms);

        }

        public static T Deserialize<T>(this string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                return default(T);

            return JsonConvert.DeserializeObject<T>(item);
        }

    }
}
