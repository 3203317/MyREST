using System;

using log4net.Layout;

namespace Foreworld.Log
{
    public class LogLayout : PatternLayout
    {
        public LogLayout()
        {
            this.AddConverter("property", typeof(LogInfoPatternConverter));
        }
    }
}
