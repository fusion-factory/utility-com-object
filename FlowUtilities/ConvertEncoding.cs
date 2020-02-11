using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace FlowUtilities
{
    /// <summary>
    /// functions to convert between various character encodings
    /// </summary>
    public class ConvertEncoding
    {

        /// <summary>
        /// convert Widestring (Unicode) to win1252 encoding
        /// </summary>
        /// <param name="ws">widestring to be encoded</param>
        /// <returns>win1252 encoded string</returns>
        public string WideStringToWin1252(string ws)
        {
            if (ws == null || ws.Length == 0)
                return "";

            Encoding ec1252 = Encoding.GetEncoding(1252);
            byte[] as_bytes = ec1252.GetBytes(ws);
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < as_bytes.Length; ++i)
                s.Append((char)as_bytes[i]);
            return s.ToString();
        }

        /// <summary>
        /// convert win1252 to Widestring (unicode) encoding
        /// </summary>
        /// <param name="win_1252">win1252 data to be converted</param>
        /// <returns>unicode encoded widestring</returns>
        public string Win1252ToWideString(string win_1252)
        {
            if (win_1252 == null || win_1252.Length == 0)
                return "";

            byte[] as_bytes = new byte[win_1252.Length];
            for (int i = 0; i < win_1252.Length; ++i)
                as_bytes[i] = (byte)win_1252[i];
            Encoding ec1252 = Encoding.GetEncoding(1252);
            return ec1252.GetString(as_bytes);
        }

        /// <summary>
        /// convert Widestring (unicode) to utf8 encoding
        /// </summary>
        /// <param name="ws">Widestring to be encoded</param>
        /// <returns>utf8 encoded string</returns>
        public string WideStringToUTF8(string ws)
        {
            if (ws == null || ws.Length == 0)
                return "";

            Encoding ecUTF8 = Encoding.UTF8;
            byte[] as_bytes = ecUTF8.GetBytes(ws);
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < as_bytes.Length; ++i)
                s.Append((char)as_bytes[i]);
            return s.ToString();
        }

        /// <summary>
        /// convert UTF8 encoded string to Widestring (unicode)
        /// </summary>
        /// <param name="utf8">utf8 encoded data</param>
        /// <returns>Widestring (unicode) string</returns>
        public string UTF8ToWideString(string utf8)
        {
            if (utf8 == null || utf8.Length == 0)
                return "";

            byte[] as_bytes = new byte[utf8.Length];
            for (int i = 0; i < utf8.Length; ++i)
                as_bytes[i] = (byte)utf8[i];
            Encoding ecUTF8 = Encoding.UTF8;
            return ecUTF8.GetString(as_bytes);
        }

        /// <summary>
        /// convert win1252 string to utf8 string
        /// </summary>
        /// <param name="win_1252">win1252 string</param>
        /// <returns>utf8 string</returns>
        public string Win1252ToUTF8(string win_1252)
        {
            return WideStringToUTF8(Win1252ToWideString(win_1252));
        }

        /// <summary>
        /// convert utf8 string to win1252
        /// </summary>
        /// <param name="utf8">utf8 string</param>
        /// <returns>win1252 string</returns>
        public string UTF8ToWin1252(string utf8)
        {
            return WideStringToWin1252(UTF8ToWideString(utf8));
        }

        private string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }

        /// <summary>
        /// convert utf8 encoded string to ascii with JSON unicode escape sequences
        /// </summary>
        /// <param name="utf8">utf8 encoded string</param>
        /// <returns>ascii string with json escape sequences</returns>
        public string UTF8ToJson(string utf8)
        {
            if (utf8 == null || utf8.Length == 0)
                return "";

            return EncodeNonAsciiCharacters(UTF8ToWideString(utf8));
        }

        /// <summary>
        /// convert ascii string with JSON unciode escape sequences to utf8 encoding
        /// </summary>
        /// <param name="jstring">ascci string with JSON unicode escape sequences</param>
        /// <returns>utf8 encoded string</returns>
        public string JsonToUTF8(string jstring)
        {
            if (jstring == null || jstring.Length == 0)
                return "";

            return WideStringToUTF8(DecodeEncodedNonAsciiCharacters(jstring));
        }
    }
}
