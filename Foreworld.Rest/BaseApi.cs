using System;
using System.Web;
using System.Web.SessionState;
using System.Reflection;

namespace Foreworld.Rest
{
    public abstract class BaseApi : IHttpHandler, IRequiresSessionState
    {
        public abstract void ProcessRequest(HttpContext @context);

        public bool IsReusable { get { return false; } }

        public DateTime _startTime = DateTime.Now;

        public static string AssemblyCompany
        {
            get
            {
                object[] __attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                return __attributes.Length == 0 ? "foreworld.net" : ((AssemblyCompanyAttribute)__attributes[0]).Company;
            }
        }
    }
}
