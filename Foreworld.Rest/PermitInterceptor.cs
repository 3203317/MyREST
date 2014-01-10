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
    public class PermitInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(PermitInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            HttpRequest __request = @context.Request;
            ////获取Cmd实例
            //ICmd __cmd = CmdManager.INSTANCE.GetCmd(__request.QueryString["command"].Trim());

            //if (__cmd.Access == AccessLevel.PROTECTED)
            //{
            //    //验证用户对Cmd是否有访问的权限
            //    successor.RequestInterceptor(@context);
            //}
            //else
            //{
            //    successor.RequestInterceptor(@context);
            //}
        }
    }
}
