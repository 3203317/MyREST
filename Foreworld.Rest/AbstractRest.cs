using System;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Resources;

using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public abstract class AbstractRest : IHttpHandler, IRequiresSessionState
    {
        public virtual void ProcessRequest(HttpContext @context)
        {
            SetData2Context(@context);

            HttpResponse __response = @context.Response;
            __response.ClearHeaders();
            __response.AddHeader("EMAIL", "huangxin@foreworld.net");
            __response.Charset = "UTF-8";
            __response.Buffer = true;
        }

        public virtual bool IsReusable { get { return false; } }

        public DateTime _startTime = DateTime.Now;

        private string _companyName = string.Empty;
        public string CompanyName
        {
            get
            {
                if (0 == _companyName.Length)
                {
                    object[] __attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                    _companyName = 0 == __attributes.Length ? "foreworld.net" : ((AssemblyCompanyAttribute)__attributes[0]).Company;
                }
                return _companyName;
            }
        }

        private string _version = string.Empty;
        protected string Version
        {
            get
            {
                if (0 == _version.Length)
                {
                    _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
                return _version;
            }
        }

        private ResultMapper _resultMapper;
        protected ResultMapper ResultMapper
        {
            get { return _resultMapper; }
            set { _resultMapper = value; }
        }

        /// <summary>
        /// 获取Rest地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual string GetRestUrl(HttpContext @context)
        {
            string __rawUrl = @context.Request.RawUrl;
            string __restUrl = __rawUrl.Substring(0, __rawUrl.LastIndexOf('.'));
            return __restUrl;
        }

        /// <summary>
        /// 获取数据放置上下文
        /// </summary>
        /// <param name="context"></param>
        private void SetData2Context(HttpContext @context)
        {
            string __restUrl = GetRestUrl(@context).ToLower();

            ICmd __cmd = CmdManager.INSTANCE.GetCmd(__restUrl);

            if (VerifyCmd(__cmd))
            {
                Parameter __parameter = new Parameter();
                __parameter.HttpContext = @context;

                _resultMapper = new ResultMapper();
                __parameter.ResultMapper = _resultMapper;

                __cmd.Execute(__parameter);
            }
        }

        /// <summary>
        /// 验证Cmd
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private bool VerifyCmd(ICmd @cmd)
        {
            bool __result = true;

            if (AccessLevel.PUBLIC == @cmd.Access)
            {
                __result = true;
            }

            return __result;
        }
    }
}
