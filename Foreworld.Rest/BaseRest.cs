using System;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Resources;

using Foreworld.Cmd;
using Foreworld.Log;

namespace Foreworld.Rest
{
    public abstract class BaseRest : IHttpHandler, IRequiresSessionState
    {
        public virtual void ProcessRequest(HttpContext @context)
        {
            HttpResponse response = @context.Response;
            response.ClearHeaders();
            response.AddHeader("EMAIL", "huangxin@foreworld.net");
            response.AddHeader("FOREWORLD.NET", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            response.Charset = "UTF-8";
            response.Buffer = true;

            SetData2Context(@context);
        }

        public virtual bool IsReusable { get { return false; } }

        public DateTime _startTime = DateTime.Now;

        protected ResultMapper ResultMapper { get; set; }

        protected virtual RequestType RequestType { get { return RequestType.JSON; } }

        /// <summary>
        /// 获取资源名称
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual string GetResourceName(HttpContext @context)
        {
            HttpRequest request = @context.Request;

            string resName = request.QueryString["_resName_"];
            if (null == resName)
            {
                string rawUrl_3 = request.RawUrl;
                resName = rawUrl_3.Substring(0, rawUrl_3.LastIndexOf('.'));
            }

            resName = resName.Trim();
            return resName;
        }

        /// <summary>
        /// 获取数据并设置上下文
        /// </summary>
        /// <param name="context"></param>
        private void SetData2Context(HttpContext @context)
        {
            string resName = GetResourceName(@context).ToLower();

            if (0 == resName.Length)
            {
                ResultMapper mapper_3 = new ResultMapper();
                mapper_3.Msg = "参数传递异常";
                ResultMapper = mapper_3;
            }
            else
            {
                Parameter parameter_3 = new Parameter();
                parameter_3.HttpContext = @context;
                parameter_3.RequestType = RequestType;
                parameter_3.LogInfo = new LogInfo(Util.GetClientIP(@context.Request));

                ResultMapper mapper_3 = CmdManager.INSTANCE.Exec(resName, parameter_3);
                ResultMapper = mapper_3;
            }
        }
    }
}
