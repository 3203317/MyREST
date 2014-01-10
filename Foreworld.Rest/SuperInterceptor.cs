using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Reflection;
using System.Configuration;
using System.IO;

using Newtonsoft.Json;
using log4net;

namespace Foreworld.Rest
{
    class SuperInterceptor : IHttpModule
    {
        private DateTime _startTime;
        private static readonly ILog _log = LogManager.GetLogger(typeof(SuperInterceptor));

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            _startTime = DateTime.Now;
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication __application = (HttpApplication)sender;
            if (!__application.Request.CurrentExecutionFilePath.EndsWith("/Api.ashx", StringComparison.OrdinalIgnoreCase)) return;


            string __result = "{\"foreworld.net\":\"" + Assembly.GetExecutingAssembly().GetName().Version + "\"";

            HttpContext __context = __application.Context;
            if (__context.Items["result"] != null && __context.Items["result"].ToString() != string.Empty)
            {
                string __result_3 = __context.Items["result"].ToString();
                __result += "," + __result_3.Substring(1, __result_3.Length - 2);
            }
            //页面执行时间（秒）
            __result += ",\"processed\":\"" + (DateTime.Now - _startTime).TotalSeconds.ToString() + "\"}";


            HttpResponse __response = __application.Response;
            __response.ContentType = "text/plain";
            __response.AddHeader("Email", "huangxin@foreworld.net");
            __response.Charset = "UTF-8";
            __response.Write(__result);
        }

        private void Application_Error(object sender, EventArgs e)
        {
            HttpApplication __application = (HttpApplication)sender;
            if (!__application.Request.CurrentExecutionFilePath.EndsWith("/Api.ashx", StringComparison.OrdinalIgnoreCase)) return;

            __application.CompleteRequest();

            _log.Error(__application.Server.GetLastError());

            HttpContext __context = __application.Context;

            //JSON对象创建
            StringWriter __sw = new StringWriter();
            JsonWriter __jw = new JsonWriter(__sw);

            __jw.WriteStartObject();

            __jw.WritePropertyName("opt");
            __jw.WriteValue("f");

            __jw.WritePropertyName("msg");
            __jw.WriteValue(__application.Server.GetLastError().Message);

            __jw.WritePropertyName("tip");
            __jw.WriteValue(__context.Items["err"] == null ? "" : __context.Items["err"].ToString());

            __jw.WritePropertyName("code");
            __jw.WriteValue(__context.Items["code"] == null ? "" : __context.Items["code"].ToString());

            __jw.WriteEndObject();

            __jw.Flush();

            string __result = __sw.GetStringBuilder().ToString();

            __jw.Close();
            __sw.Close();
            __sw.Dispose();


            __context.Items["result"] = "," + __result.Substring(1, __result.Length - 1);
            //HttpContext.Current.Server.ClearError();
        }

        #region IHttpModule 成员
        public void Dispose() { }


        public void Init(HttpApplication application)
        {
            application.Error += new EventHandler(Application_Error);
            application.BeginRequest += new EventHandler(Application_BeginRequest);
            application.EndRequest += new EventHandler(Application_EndRequest);
        }
        #endregion
    }
}