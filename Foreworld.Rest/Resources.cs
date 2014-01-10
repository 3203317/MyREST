using System;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Resources;

namespace Foreworld.Rest
{
    public abstract class Resources : IHttpHandler, IRequiresSessionState
    {
        public abstract ResourceManager ResourceManager { get; }

        public void ProcessRequest(HttpContext @context)
        {
            HttpResponse __response = @context.Response;
            HttpRequest __request = @context.Request;
            string __resName = __request.QueryString["name"].Trim();

            /* 判断资源名称是否为空 */
            if ("".Equals(__resName))
            {
                __response.Redirect("404.html");
                __response.End();
            }

            __response.ClearHeaders();
            __response.AddHeader("EMAIL", "huangxin@foreworld.net");
            __response.Charset = "UTF-8";
            __response.ContentType = "text/html";
            __response.Buffer = true;

            //__response.Write("<a href='http://www.foreworld.net/a1=" + __resName + "'>洪荒</a>");

            /* 获取模板字符串 */
            string __tplStr = this.GetTplStr(__resName);

            if ("".Equals(__tplStr))
            {
                __response.Redirect("~/404.html?msg=No Page Found");
                __response.End();
            }

            /* 原始URL */
            string __rawUrl = __request.RawUrl;
            string __uri = __rawUrl.Substring(0, __rawUrl.LastIndexOf('/')).ToLower();


            for (int i = 0; i < __request.QueryString.Count; i++)
            {

                __response.Write(__request.QueryString[i] + "<br/>");
            }



            /* 模板与资源合并 */
            string __resultStr = __tplStr + __uri;
            __response.Write(__resultStr);
            __response.End();
        }

        public bool IsReusable { get { return false; } }

        public DateTime _startTime = DateTime.Now;

        public static string AssemblyCompany
        {
            get
            {
                object[] __attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                string __resultStr = __attributes.Length == 0 ? "foreworld.net" : ((AssemblyCompanyAttribute)__attributes[0]).Company;
                return __resultStr;
            }
        }

        /// <summary>
        /// 获取模板字符串
        /// </summary>
        /// <param name="tplName">模板名称</param>
        /// <returns></returns>
        private string GetTplStr(string @tplName)
        {
            object __obj = ResourceManager.GetObject("_" + @tplName);
            if (null == __obj) return string.Empty;
            string __tplStr = __obj.ToString();
            return __tplStr;
        }
    }
}
