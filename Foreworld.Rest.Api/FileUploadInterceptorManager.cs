using System;
using System.Web;

using log4net;

using Foreworld.Rest;
using Foreworld.Log;

namespace Foreworld.Rest.Api
{
    class FileUploadInterceptorManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FileUploadInterceptorManager));
        private static LogInfo _logInfo = new LogInfo();

        private FileUploadInterceptorManager()
        {
            _logInfo.Msg = "FileUploadInterceptorManager初始化...";
            _log.Info(_logInfo);
            Init();
        }

        private static volatile FileUploadInterceptorManager _instance;
        private static object _syncRoot = new Object();

        public static FileUploadInterceptorManager INSTANCE
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null) _instance = new FileUploadInterceptorManager();
                    }
                }
                return _instance;
            }
        }

        private Interceptor _timeParamInterceptor;

        private Interceptor _formParamsInterceptor;
        private Interceptor _sessionInterceptor;
        private Interceptor _resultInterceptor;

        private Interceptor _fileCategoryInterceptor;
        private Interceptor _cmdInterceptor;
        private Interceptor _cookieInterceptor;
        private Interceptor _permitInterceptor;

        private void Init()
        {
            _timeParamInterceptor = new TimeParamInterceptor();

            _cmdInterceptor = new CmdInterceptor();
            _fileCategoryInterceptor = new FileCategoryInterceptor();
            _cookieInterceptor = new CookieInterceptor();

            _formParamsInterceptor = new FormParamsInterceptor();
            _sessionInterceptor = new SessionInterceptor();
            _permitInterceptor = new PermitInterceptor();
            _resultInterceptor = new ResultInterceptor();



            _timeParamInterceptor.SetSuccessor(_cmdInterceptor);
            _cmdInterceptor.SetSuccessor(_fileCategoryInterceptor);

            _fileCategoryInterceptor.SetSuccessor(_cookieInterceptor);

            _cookieInterceptor.SetSuccessor(_formParamsInterceptor);

            _formParamsInterceptor.SetSuccessor(_sessionInterceptor);

            _sessionInterceptor.SetSuccessor(_permitInterceptor);
            _permitInterceptor.SetSuccessor(_resultInterceptor);

        }

        public void Run(HttpContext @context)
        {
            _timeParamInterceptor.RequestInterceptor(@context);
        }
    }
}
