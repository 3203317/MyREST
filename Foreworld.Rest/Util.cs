using System;
using System.Web;
using System.IO;
using System.Data;

using Newtonsoft.Json;

using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class Util
    {
        /// <summary>
        /// 判断是否是本地站点请求
        /// </summary>
        /// <returns></returns>
        public static bool CheckRequestFromLocal()
        {
            string __server1 = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            string __server2 = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            return !string.IsNullOrEmpty(__server1) ? (__server1.Substring(7, __server2.Length) == __server2) : false;
        }

        /// <summary>
        /// 异常日志记录
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="status"></param>
        /// <param name="msg">错误描述</param>
        public static void ExceptionLog(HttpContext @context, string @status, string @msg)
        {
            //JSON对象创建
            StringWriter __sw = new StringWriter();
            JsonWriter __jw = new JsonWriter(__sw);

            __jw.WriteStartObject();

            __jw.WritePropertyName("status");
            __jw.WriteValue(@status);

            __jw.WritePropertyName("msg");
            __jw.WriteValue(@msg);

            //__jw.WritePropertyName("code");
            //__jw.WriteValue(@code);

            __jw.WriteEndObject();

            __jw.Flush();

            string __result = __sw.GetStringBuilder().ToString();

            __jw.Close();
            __sw.Close();
            __sw.Dispose();

            @context.Items["result"] = __result;
        }

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIP(HttpRequest @request)
        {
            string ip = "0.0.0.0";
            try
            {
                ip = @request.ServerVariables["http_VIA"] == null ? @request.UserHostAddress : @request.ServerVariables["http_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            }
            catch { }

            return ip;
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dformat"></param>
        /// <returns></returns>
        public static string DataConversion(object @data, DFormat @dformat)
        {
            string __result = string.Empty;
            string __objType = @data.GetType().ToString();

            switch (__objType)
            {
                case "System.Data.DataSet":
                    __result = DataSetToString((DataSet)@data, @dformat);
                    break;
                default:
                    __result = @data.ToString();
                    break;
            }
            return __result;
        }

        /// <summary>
        /// DataSet转换字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dformat"></param>
        /// <returns></returns>
        private static string DataSetToString(DataSet @data, DFormat @dformat)
        {
            //JSON对象创建
            StringWriter __sw = new StringWriter();
            JsonWriter __jw = new JsonWriter(__sw);

#if DEBUG
            __jw.Formatting = Formatting.Indented;
#endif
            __jw.WriteStartObject();

            DataTable __dt = @data.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            DataColumnCollection __columns = __dt.Columns;

            if (@dformat != DFormat.DOJO)
            {
                __jw.WritePropertyName("fields");

                __jw.WriteStartArray();
                for (int __i_3 = 1, __j_3 = __columns.Count; __i_3 < __j_3; __i_3++)
                {
                    __jw.WriteValue(__columns[__i_3].ToString());
                }
                __jw.WriteEndArray();
            }

            __jw.WritePropertyName(@dformat == DFormat.DOJO ? "items" : "rows");
            __jw.WriteStartArray();


            if (@dformat == DFormat.DOJO)
            {
                for (int __i_3 = 0, __j_3 = __rows.Count, __k_3 = __columns.Count; __i_3 < __j_3; __i_3++)
                {
                    __jw.WriteStartObject();

                    DataRow __row_4 = __rows[__i_3];

                    for (int __i_5 = 0; __i_5 < __k_3; __i_5++)
                    {
                        __jw.WritePropertyName(__columns[__i_5].ToString());
                        __jw.WriteValue(__row_4[__i_5].ToString());
                    }

                    __jw.WriteEndObject();
                }
            }
            else
            {
                for (int __i_3 = 0, __j_3 = __rows.Count, __k_3 = __columns.Count; __i_3 < __j_3; __i_3++)
                {
                    __jw.WriteStartObject();
                    DataRow __row_4 = __rows[__i_3];

                    __jw.WritePropertyName("id");
                    __jw.WriteValue(__row_4[0].ToString());

                    __jw.WritePropertyName("data");
                    __jw.WriteStartArray();
                    /* id */
                    __jw.WriteValue(__row_4[0].ToString());
                    /* checkbox */
                    __jw.WriteValue(0);
                    for (int __i_5 = 1; __i_5 < __k_3; __i_5++)
                    {
                        __jw.WriteValue(__row_4[__i_5].ToString());
                    }
                    __jw.WriteEndArray();

                    __jw.WriteEndObject();
                }
            }

            __jw.WriteEndArray();
            __jw.WriteEndObject();

            __jw.Flush();

            string __result = __sw.GetStringBuilder().ToString();

            @data.Clear();
            @data.Dispose();
            __jw.Close();
            __sw.Close();
            __sw.Dispose();

            return __result;
        }
    }
}
