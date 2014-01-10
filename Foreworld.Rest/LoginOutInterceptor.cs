using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Reflection;
using System.Configuration;

using log4net;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;

namespace Foreworld.Rest
{
    public class LoginOutInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LoginOutInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            HttpRequest __request = @context.Request;
            string __command = __request.QueryString["command"].Trim();

            if (__command == "login")
            {
                JavaScriptObject __userObj_3 = (JavaScriptObject)JavaScriptConvert.DeserializeObject(@context.Items["result"].ToString());

                if (!__userObj_3.ContainsKey("status"))
                {
                    JavaScriptArray __items_4 = (JavaScriptArray)__userObj_3["items"];
                    string __userData_4 = JavaScriptConvert.SerializeObject(__items_4[0]);

                    string __username_4 = __request["username"].Trim();
                    int __version_4 = Assembly.GetExecutingAssembly().GetName().Version.Major;
                    int __expiration_4 = Convert.ToInt32(ConfigurationSettings.AppSettings["timeout"].Trim());

                    FormsAuthenticationTicket __ticket_4 = new FormsAuthenticationTicket(__version_4, __username_4, DateTime.Now, DateTime.Now.AddHours(__expiration_4), false, __userData_4, "/");
                    string __encryptTicket_4 = FormsAuthentication.Encrypt(__ticket_4);
                    HttpCookie __cookie_4 = new HttpCookie(FormsAuthentication.FormsCookieName, __encryptTicket_4);
                    //__cookie_4.Expires = __ticket_4.Expiration;
                    @context.Response.Cookies.Add(__cookie_4);
                }
            }
            else if (__command == "logout")
            {
                FormsAuthentication.SignOut();
            }
        }
    }
}
