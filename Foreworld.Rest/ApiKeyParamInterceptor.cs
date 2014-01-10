using System;
using System.Web;

using log4net;

using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class ApiKeyParamInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ApiKeyParamInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            object __apikeyObj = @context.Request.QueryString["apikey"];
            string __apikey = __apikeyObj == null ? string.Empty : __apikeyObj.ToString().Trim();

            if (__apikey.Length > 0)
            {
                successor.RequestInterceptor(@context);
            }
            else
            {
                Util.ExceptionLog(@context, Status.FAILURE, string.Format(Resource.err_paramIsNotEmpty, "apikey"));
            }
        }
    }
}
