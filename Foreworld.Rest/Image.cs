using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Resources;

namespace Foreworld.Rest
{
    public class Image : BaseRest
    {
        public override void ProcessRequest(HttpContext @context)
        {
            /* 模板与资源合并 */
            string __jsonStr = "aaaaa";

            HttpResponse __response = @context.Response;
            __response.ContentType = "text/plain";
            __response.Buffer = true;

            __response.Write(__jsonStr);
            __response.End();
        }

        /// <summary>
        /// 数据合并模板
        /// </summary>
        /// <returns></returns>
        private string Data2Json()
        {
            string __jsonStr = "{'foreworld.net':'2.0'}";

            return __jsonStr;
        }
    }
}
