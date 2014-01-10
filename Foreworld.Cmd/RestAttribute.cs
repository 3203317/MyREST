using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RestAttribute : Attribute
    {
        public RestAttribute(string @name)
        {
            this.Name = @name;
        }

        public string Name { get; set; }
    }
}
