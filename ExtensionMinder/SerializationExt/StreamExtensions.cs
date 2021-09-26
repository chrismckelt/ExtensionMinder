using System;
using System.IO;

namespace ExtensionMinder.SerializationExt
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            stream.Position = 0;
            var buffer = new byte[stream.Length];
            var totalBytesCopied = 0;
            for (totalBytesCopied = 0; totalBytesCopied < stream.Length; )
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            return buffer;
        }

      public static string ConvertToString(this Stream stream)
      {
        if (stream == null) throw new Exception("No stream available for the request");
        using (var sr = new StreamReader(stream))
        {
          return sr.ReadToEnd();
        }
      }

      public static void ConvertToFile(this Stream stream, string filepath)
      {
        var file = new FileInfo(filepath);

        file.Directory?.Create();

        using (var fileStream = System.IO.File.Create(filepath))
        {
          stream.CopyTo(fileStream);
        }
      }
  }
}