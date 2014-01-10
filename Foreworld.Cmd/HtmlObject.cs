using System;
using System.Collections.Generic;
using System.Text;

using NVelocity.Context;

namespace Foreworld.Cmd
{
    public class HtmlObject
    {
        public string Template { get; set; }

        public IContext Context { get; set; }
    }
}
