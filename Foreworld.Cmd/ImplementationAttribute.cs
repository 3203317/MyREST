using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd
{
    public enum OptType
    {
        C,
        R,
        U,
        D
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ImplementationAttribute : Attribute
    {
        public ImplementationAttribute(string name)
        {
            _name = name;
        }

        private string _name;
        private string _version = "1.0.0.0";
        private string _author = "huangxin@foreworld.net";
        private string _description = string.Empty;
        private bool _includeInApiDoc = true;

        /// <summary>
        /// 公开Api, 对外直接访问, 可以不使用Session验证
        /// </summary>
        private bool _open = false;
        private OptType _opt = OptType.R;


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool IncludeInApiDoc
        {
            get { return _includeInApiDoc; }
            set { _includeInApiDoc = value; }
        }

        public bool Open
        {
            get { return _open; }
            set { _open = value; }
        }

        private OptType Opt
        {
            get { return _opt; }
            set { _opt = value; }
        }

    }
}
