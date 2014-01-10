using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Foreworld.Cmd
{
    [Serializable]
    public class ResultMapper
    {
        public ResultMapper()
        {
            object[] attrObjs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            _webSite = 0 == attrObjs.Length ? "foreworld.net" : ((AssemblyCompanyAttribute)attrObjs[0]).Company;

            _version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public bool Success { get; set; }
        public Object Data { get; set; }
        public string Msg { get; set; }
        public string Code { get; set; }

        private string _webSite;
        public string WebSite
        {
            get { return _webSite; }
        }

        private string _version;
        public string Version
        {
            get { return _version; }
        }
    }
}
