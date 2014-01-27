using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        private string GetVltTemplate(string templateName)
        {
            string resultStr = string.Empty;
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(this.GetType().Assembly.GetName().Name + ".tpl." + templateName + ".html");
            if (null != stream)
            {
                byte[] byte_3 = new byte[stream.Length];
                stream.Read(byte_3, 0, byte_3.Length);
                resultStr = Encoding.UTF8.GetString(byte_3); //将byte数组转换为string
            }
            return resultStr;
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
                resultStr = GetVltTemplate(tplName);

                if (0 < resultStr.Length)
                {
                    Regex regex_4 = new Regex("#parse\\(\\'(.*)\\.html\\'\\);");
                    MatchCollection matches_4 = regex_4.Matches(resultStr);

                    for (int i = 0, j = matches_4.Count; i < j; i++)
                    {
                        string match_5 = matches_4[i].ToString();
                        string tplName_5 = matches_4[i].Groups[1].Value.Replace('/', '.');

                        resultStr = resultStr.Replace(match_5, GetVltTemplate(tplName_5));
                    }
                }
            }
            return resultStr;
        }
    }
}
