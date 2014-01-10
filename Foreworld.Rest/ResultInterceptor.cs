#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.IO;

using log4net;
using Newtonsoft.Json;

using Foreworld.Cmd;
using Foreworld.Log;

namespace Foreworld.Rest
{
    public class ResultInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ResultInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            HttpRequest __request = @context.Request;
            Parameter __parameter = new Parameter();

            //表单参数
            __parameter.Parameters = (Dictionary<string, string>)@context.Items["parameters"];

            //输出格式
            //object __responseObj = __request.QueryString["response"];
            //__parameter.Format = (__responseObj == null || __responseObj.ToString().Trim().ToLower() == "json") ? Response.JSON : Response.XML;

            //数据格式,dojo或dhx
            object __formatObj = __request.QueryString["format"];
            DFormat __dformat = (__formatObj == null || __formatObj.ToString().Trim().ToLower() == "dojo") ? DFormat.DOJO : DFormat.DHX;

            //用户Session
            object __userInfo = @context.Items["userInfo"];
            if (__userInfo != null)
            {
                __parameter.UserInfo = (JavaScriptObject)JavaScriptConvert.DeserializeObject(__userInfo.ToString());
                __parameter.LogInfo = new LogInfo(Util.GetClientIP(__request));
            }
            else
            {
                __parameter.LogInfo = new LogInfo(Util.GetClientIP(__request));
            }

            //
            __parameter.HttpContext = @context;

            ////获取Cmd命令
            //ICmd __cmd = CmdManager.INSTANCE.GetCmd(__request.QueryString["command"].Trim());
            //string __result = Util.DataConversion(__cmd.Execute(__parameter), __dformat);
            ////__result = "," + __result.Substring(1, __result.Length - 2);

            //@context.Items["result"] = __result;

            if (successor != null) successor.RequestInterceptor(@context);
        }
    }
}
