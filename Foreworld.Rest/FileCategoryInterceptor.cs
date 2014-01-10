using System;
using System.Web;
using System.Web.Security;

using log4net;

using Foreworld.Cmd;
using Foreworld.Rest;

namespace Foreworld.Rest
{
    public class FileCategoryInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FileCategoryInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            ////获取Cmd实例
            //ICmd __cmd = CmdManager.INSTANCE.GetCmd(@context.Request.QueryString["command"].Trim());

            //if (__cmd.Category == Category.FILE)
            //{
            //    successor.RequestInterceptor(@context);
            //}
            //else
            //{
            //    Util.ExceptionLog(@context, Status.FAILURE, Resource.err_noAccess);
            //}
        }
    }
}
