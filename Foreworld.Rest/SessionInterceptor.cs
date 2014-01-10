using System;
using System.Web;
using System.Web.Security;

using log4net;

using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class SessionInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SessionInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            HttpCookie __cookie = @context.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (__cookie != null)
            {
                FormsAuthenticationTicket __ticket_3 = FormsAuthentication.Decrypt(__cookie.Value);
                @context.Items["userInfo"] = __ticket_3.UserData;
            }

            successor.RequestInterceptor(@context);
        }

        ///// <summary>
        ///// 判断Session存在
        ///// </summary>
        ///// <returns></returns>
        //private bool hasSession()
        //{
        //    HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        //    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
        //    System.Diagnostics.Debug.WriteLine("-----------------------------------");
        //    System.Diagnostics.Debug.WriteLine(ticket.UserData);
        //    System.Diagnostics.Debug.WriteLine(HttpContext.Current.User.Identity.Name);
        //    return false;
        //}
    }
}
