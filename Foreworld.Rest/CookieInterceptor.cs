using System;
using System.Web;
using System.Web.Security;

using log4net;

using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class CookieInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CookieInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            HttpRequest __request = @context.Request;
            string __command = __request.QueryString["command"].Trim();

            HttpCookie __cookie = __request.Cookies[FormsAuthentication.FormsCookieName];

            if (__command != "login")
            {
                //if ((__cookie == null || !@context.User.Identity.IsAuthenticated) && CmdManager.INSTANCE.GetCmd(__command).Access == AccessLevel.PROTECTED)
                //{
                //    Util.ExceptionLog(@context, Status.TIMEOUT, Resource.err_noSession);
                //}
                //else
                //{
                //    successor.RequestInterceptor(@context);
                //}
            }
            else
            {
                if (__cookie == null || !@context.User.Identity.IsAuthenticated)
                {
                    successor.RequestInterceptor(@context);
                }
                else
                {
                    Util.ExceptionLog(@context, Status.SUCCESS, Resource.err_notReLogin);
                }
            }
        }
    }
}