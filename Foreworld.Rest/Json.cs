using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Resources;

using Foreworld.Cmd;
using Newtonsoft.Json;

namespace Foreworld.Rest
{
    public class Json : BaseRest
    {
        public override void ProcessRequest(HttpContext @context)
        {
            base.ProcessRequest(@context);

            HttpResponse response = @context.Response;
            response.ContentType = "text/plain";

            /* 获取结果集 */
            string resultStr = Obj2Str();

            response.Write(resultStr);
            response.End();
        }

        /// <summary>
        /// 获取结果集
        /// </summary>
        /// <returns></returns>
        private string Obj2Str()
        {
            string resultStr = JavaScriptConvert.SerializeObject(ResultMapper);
            /*JavaScriptObject jsObj = (JavaScriptObject)JavaScriptConvert.DeserializeObject(resultStr);
            jsObj.Add(CompanyName, Version);
            resultStr = JavaScriptConvert.SerializeObject(jsObj);*/
            return resultStr;
        }
    }
}
