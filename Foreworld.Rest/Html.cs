using System;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Resources;
using System.Text;
using System.IO;

using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity.Context;

using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class Html : BaseRest
    {
        private static volatile VelocityEngine _vltEngine;
        private static object _syncObj = new Object();
        private static readonly string _htmlTemplate = @"<!DOCTYPE HTML>
<HTML>
    <HEAD>
        <TITLE>Error Page</TITLE>
        <meta http-equiv='content-type' content='text/html;charset=utf-8'>
    </HEAD>
    <BODY>
        <h1>Unknown Error Page.</h1>
        <h2>Please Visit <a href='http://www.foreworld.net'>http://www.foreworld.net</a>.</h2>
    </BODY>
</HTML>";

        public Html()
        {
            Init();
        }

        private void Init()
        {
            if (null == _vltEngine)
            {
                lock (_syncObj)
                {
                    if (null == _vltEngine)
                    {
                        _vltEngine = new VelocityEngine();
                        _vltEngine.SetProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
                        _vltEngine.SetProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
                        _vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE, true);
                        _vltEngine.SetProperty("file.resource.loader.modificationCheckInterval", (Int64)30);
                        _vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, HttpContext.Current.Server.MapPath("~/App_Data/"));
                        _vltEngine.Init();
                    }
                }
            }
        }

        protected override RequestType RequestType { get { return RequestType.HTML; } }

        public override void ProcessRequest(HttpContext @context)
        {
            base.ProcessRequest(@context);

            /* 模板与资源合并 */
            string resultStr = DataTplMerge();

            HttpResponse response = @context.Response;
            response.ContentType = "text/html";
            response.Buffer = true;

            response.Write(resultStr);
            response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private HtmlObject GetHtmlObject()
        {
            HtmlObject htmlObj = null;
            if (!ResultMapper.Success && null == ResultMapper.Data)
            {
                htmlObj = new HtmlObject();
                htmlObj.Context = new VelocityContext();
                htmlObj.Template = _htmlTemplate;
            }
            else
            {
                htmlObj = (HtmlObject)ResultMapper.Data;
            }
            return htmlObj;
        }

        /// <summary>
        /// 数据模板合并
        /// </summary>
        /// <returns></returns>
        private string DataTplMerge()
        {
            HtmlObject htmlObj = GetHtmlObject();
            StringWriter vltWriter = new StringWriter();
            _vltEngine.Evaluate(htmlObj.Context, vltWriter, null, htmlObj.Template);

            string resultStr = vltWriter.GetStringBuilder().ToString();
            return resultStr;
        }
    }
}
