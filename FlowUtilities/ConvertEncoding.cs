using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace FlowUtilities
{
    public class ConvertEncoding
    {
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

        public string Win1252ToUTF8(string win_1252)
        {
            return WideStringToUTF8(Win1252ToWideString(win_1252));
        }
    }
}
