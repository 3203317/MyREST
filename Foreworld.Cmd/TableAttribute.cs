using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string @name)
        {
            this.Name = @name;
        }

        public string Name { get; set; }
    }
}
