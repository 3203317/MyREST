using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;

using log4net;
using Newtonsoft.Json;

using Foreworld.Cmd;
using Foreworld.Log;

namespace Foreworld.Rest
{
    public class ApiKeyInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ApiKeyInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            HttpRequest __request = @context.Request;
            //获取Cmd实例
            //ICmd __cmd = CmdManager.INSTANCE.GetCmd(__request.QueryString["command"].Trim());

            //if (__cmd.Access == AccessLevel.PROTECTED)
            //{
            //    //创建命令参数组
            //    Dictionary<string, string> __parameters_3 = new Dictionary<string, string>();
            //    __parameters_3.Add("apikey", __request.QueryString["apikey"].Trim());

            //    ICmd __cmd_3 = CmdManager.INSTANCE.GetCmd("verifyUserKeys");
            //    string __result_3 = Util.DataConversion(__cmd_3.Execute(new Parameter(Response.JSON, null, new LogInfo(0, Util.GetIP(__request)), __parameters_3)), DFormat.DOJO);

            //    JavaScriptObject __userObj_3 = (JavaScriptObject)JavaScriptConvert.DeserializeObject(__result_3);

            //    if (__userObj_3.ContainsKey("status"))
            //    {
            //        @context.Items["result"] = __result_3;
            //    }
            //    else
            //    {
            //        JavaScriptArray __items_4 = (JavaScriptArray)__userObj_3["items"];
            //        @context.Items["userInfo"] = __items_4[0];

            //        successor.RequestInterceptor(@context);
            //    }
            //}
            //else
            //{
            //    successor.RequestInterceptor(@context);
            //}
        }
    }
}
