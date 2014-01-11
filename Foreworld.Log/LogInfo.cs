using System;

namespace Foreworld.Log
{
    public class LogInfo
    {
        public LogInfo() { }

        public LogInfo(string @ip)
        {
            IP = @ip;
        }

        public string UserId { get; set; }

        public string IP { get; set; }

        public string Msg { get; set; }

        public string Code { get; set; }

        public override string ToString()
        {
            return Msg;
        }
    }
}
