using System;
using System.Web;

using log4net;

using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class SignatureParamInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SignatureParamInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            object __signatureObj = @context.Request.QueryString["signature"];
            string __signature = __signatureObj == null ? string.Empty : __signatureObj.ToString().Trim();

            if (__signature.Length > 0)
            {
                successor.RequestInterceptor(@context);
            }
            else
            {
                Util.ExceptionLog(@context, Status.FAILURE, string.Format(Resource.err_paramIsNotEmpty, "signature"));
            }
        }
    }
}
