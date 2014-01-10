using System;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;

using log4net;

using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class TimeParamInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TimeParamInterceptor));

        private static readonly int _timestamp = Convert.ToInt32(ConfigurationSettings.AppSettings["timestamp"]);

        public override void RequestInterceptor(HttpContext @context)
        {
            object __tsObj = @context.Request.QueryString["ts"];
            string __ts = __tsObj == null ? string.Empty : __tsObj.ToString().Trim();

            if (__ts.Length > 0 && Regex.IsMatch(__ts, "^\\d{1,10}$") && Validate(Convert.ToInt32(__ts)))
            {
                successor.RequestInterceptor(@context);
            }
            else
            {
                Util.ExceptionLog(@context, Status.FAILURE, string.Format(Resource.err_paramValueRegex, "ts"));
            }
        }

        /// <summary>
        /// 时间有效期验证
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private bool Validate(int @timestamp)
        {
            long __timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            bool __result = (__timestamp - _timestamp) < @timestamp && @timestamp < (__timestamp + _timestamp);
            return __result;
        }
    }
}
