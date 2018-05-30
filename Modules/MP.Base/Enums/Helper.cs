using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Base.Enums
{
    public class Helper
    {
        public static string Text(Enum value)
        {
            var prefix = "";

            return "";
        }

        public static string MsgCodeText(MsgCode msgCode)
        {
            var msg = "";
            msg = string.IsNullOrEmpty(msg) ? msgCode.ToString() : msg;
            return msg;
        }
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
