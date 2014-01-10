using System;
using System.Web;

using log4net;

using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class CmdInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CmdInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            object __commandObj = @context.Request.QueryString["command"];
            string __command = __commandObj == null ? string.Empty : __commandObj.ToString().Trim();

            if (__command.Length > 0)
            {
                ////获取Cmd实例
                //ICmd __cmd_3 = CmdManager.INSTANCE.GetCmd(__command);

                //if (__cmd_3 != null)
                //{
                //    if (__cmd_3.Access != AccessLevel.PRIVATE)
                //    {
                //        successor.RequestInterceptor(@context);
                //    }
                //    else
                //    {
                //        Util.ExceptionLog(@context, Status.FAILURE, Resource.err_noPublicCmd);
                //    }
                //}
                //else
                //{
                //    Util.ExceptionLog(@context, Status.FAILURE, string.Format(Resource.err_noCommand, __command));
                //}
            }
            else
            {
                Util.ExceptionLog(@context, Status.FAILURE, string.Format(Resource.err_paramIsNotEmpty, "command"));
            }
        }
    }
}
