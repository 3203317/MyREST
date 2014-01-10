using System;
using System.Web;
using System.IO;

using Newtonsoft.Json;

namespace Foreworld.Cmd
{
    public class Util
    {
        /// <summary>
        /// 异常日志记录
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ExceptionLog(string @msg)
        {
            //JSON对象创建
            StringWriter __sw = new StringWriter();
            JsonWriter __jw = new JsonWriter(__sw);

            __jw.WriteStartObject();

            __jw.WritePropertyName("status");
            __jw.WriteValue("failure");

            __jw.WritePropertyName("msg");
            __jw.WriteValue(@msg);

            //__jw.WritePropertyName("code");
            //__jw.WriteValue(@code);

            //__jw.WritePropertyName("tip");
            //__jw.WriteValue(@tip);

            __jw.WriteEndObject();

            __jw.Flush();

            string __result = __sw.GetStringBuilder().ToString();

            __jw.Close();
            __sw.Close();
            __sw.Dispose();

            return __result;
        }
    }
}
