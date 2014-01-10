using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ResourceAttribute : Attribute
    {
        public bool Public { get; set; }
    }
}
