using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using log4net;

namespace Foreworld.Cmd
{
    public abstract class BaseRest : IRest
    {
        private Dictionary<string, string> _vltTemplate = null;

        public BaseRest()
        {
            this._vltTemplate = new Dictionary<string, string>();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(BaseRest));

        public void Destroy() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected string GetDataStr(Parameter @parameter)
        {
            HttpRequest request = @parameter.HttpContext.Request;
            string dataStr = request.Form["data"];
            return null == dataStr ? string.Empty : dataStr.Trim();
        }

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <returns></returns>
        protected string GetVltTemplate()
        {
            string resultStr = string.Empty;
            StackFrame sf = new StackFrame(1);
            string tplName = sf.GetMethod().ReflectedType.Name + "." + sf.GetMethod().Name;

            if (this._vltTemplate.ContainsKey(tplName))
            {
                resultStr = this._vltTemplate[tplName];
            }
            else
            {
                Stream stream_3 = this.GetType().Assembly.GetManifestResourceStream(this.GetType().Assembly.GetName().Name + ".tpl." + tplName + ".html");
                if (null != stream_3)
                {
                    byte[] byte_4 = new byte[stream_3.Length];
                    stream_3.Read(byte_4, 0, byte_4.Length);
                    resultStr = Encoding.UTF8.GetString(byte_4); //将byte数组转换为string
                }
            }
            return resultStr;
        }
    }
}
