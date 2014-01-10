using System;
using System.Web;
using System.Collections.Generic;

using Newtonsoft.Json;

using Foreworld.Log;

namespace Foreworld.Cmd
{
    public static class Status
    {
        public const string SUCCESS = "success";
        public const string FAILURE = "failure";
        public const string TIMEOUT = "timeout";
    }

    public enum DFormat { DOJO, DHX }

    public enum Response { XML, JSON }

    public enum AccessLevel { PUBLIC, PROTECTED, PRIVATE }

    public enum Category { INFO, FILE }

    public enum RequestType { HTML, JSON, LOOKPIC, UPLOADFILE, DOWNLOADFILE }

    public interface ICmd
    {
        AccessLevel Access { get; }

        Category Category { get; }

        string ConnectionString { get; }

        object Execute(Parameter @parameter);

        void Destroy();
    }

    public class Parameter
    {
        public Parameter() { }

        public Parameter(Response @format, JavaScriptObject @userInfo, Dictionary<string, string> @parameters)
        {
            _format = @format;
            _userInfo = @userInfo;
            _parameters = @parameters;
        }

        public Parameter(Response @format, JavaScriptObject @userInfo, LogInfo @logInfo, Dictionary<string, string> @parameters)
        {
            _format = @format;
            _userInfo = @userInfo;
            _logInfo = @logInfo;
            _parameters = @parameters;
        }

        private Response _format;

        public Response Format
        {
            get { return _format; }
            set { _format = value; }
        }

        private JavaScriptObject _userInfo;

        public JavaScriptObject UserInfo
        {
            get { return _userInfo; }
            set { _userInfo = value; }
        }

        private Dictionary<string, string> _parameters;

        public Dictionary<string, string> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        private LogInfo _logInfo;

        public LogInfo LogInfo
        {
            get { return _logInfo; }
            set { _logInfo = value; }
        }

        public HttpContext HttpContext { get; set; }

        public ResultMapper ResultMapper { get; set; }

        public RequestType RequestType { get; set; }


    }
}
