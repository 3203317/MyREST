using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Utils
{
    public class DescribeAttribute : Attribute
    {
        public DescribeAttribute(string name)
        {
            _name = name;
        }


        private string _name = string.Empty;
        private string _version = string.Empty;
        private string _author = "huangxin@foreworld.net";


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

    }
}
