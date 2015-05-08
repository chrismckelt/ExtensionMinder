using System;
using System.IO;
using System.Linq;

namespace ExtensionMinder
{
    public static class FileExtensions
    {

        public static class FileTypeExtensions
        {
            public static string Jpg = ".jpg";
            public static string Jpeg = ".jpeg";
            public static string Png = ".png";
            public static string Gif = ".gif";
            public static string Pdf = ".pdf";
            public static string Doc = ".doc";
            public static string Docx = ".docx";
            public static string Xls = ".xls";
            public static string Xlsx = ".xlsx";
            public static string Ppt = ".ppt";
            public static string Pptx = ".pptx";
            public static string Js = ".js";
            public static string Css = ".css";
            public static string Bmp = ".bmp";
            public static string Wmv = ".wmv";
            public static string Swf = ".swf";
        }

        public static string ToJavaScriptExtension(this string str, bool justdoIt = false)
        {
            if (string.IsNullOrEmpty(str)) return null;

            if (Path.HasExtension(str) && str.IsFileExtensionValid() && !justdoIt)
            {
                return Path.ChangeExtension(str, FileTypeExtensions.Js);
            }
            else
            {
                return str + FileTypeExtensions.Js;
            }
        }


        /// <summary>
        /// Checks if file extension passed as a parameter is 
        /// valid. The expected format in order for it to be valid 
        /// is ".listofCharacters", that is, a dot followed by a 
        /// set of valid file name characters. The method does not 
        /// limit the file extension to a maximum number of 
        /// characters.
        /// </summary>
        /// <param name="fExt">File extension to be 
        /// tested for format validity.</param>
        /// <returns><b>True</b> if the file extension passed is 
        /// valid and <b>false</b> otherwise.</returns>
        public static bool IsFileExtensionValid(this string fExt)
        {
            bool answer = true;
            if (!String.IsNullOrWhiteSpace(fExt)
                && fExt.Length > 1
                && fExt[0] == '.')
            {
                char[] invalidFileChars = Path.GetInvalidFileNameChars();
                foreach (char c in invalidFileChars)
                {
                    if (fExt.Contains(c))
                    {
                        answer = false;
                        break;
                    }
                }
            }
            return answer;
        }
    }
}
