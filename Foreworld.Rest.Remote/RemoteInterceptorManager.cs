using System;
using System.Web;

using log4net;

using Foreworld.Rest;
using Foreworld.Log;

namespace Foreworld.Rest.Remote
{
    class RemoteInterceptorManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(RemoteInterceptorManager));
        private static LogInfo _logInfo = new LogInfo();

        private RemoteInterceptorManager()
        {
            _logInfo.Msg = "RemoteInterceptorManager初始化...";
            _log.Info(_logInfo);
            Init();
        }

        private static volatile RemoteInterceptorManager _instance;
        private static object _syncRoot = new Object();

        public static RemoteInterceptorManager INSTANCE
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null) _instance = new RemoteInterceptorManager();
                    }
                }
                return _instance;
            }
        }

        private Interceptor _formParamsInterceptor;
        private Interceptor _resultInterceptor;

        private Interceptor _apiCategoryInterceptor;
        private Interceptor _cmdInterceptor;
        private Interceptor _apiKeyInterceptor;
        private Interceptor _permitInterceptor;

        private Interceptor _signatureParamInterceptor;

        private Interceptor _apiKeyParamInterceptor;
        private Interceptor _timeParamInterceptor;
        private Interceptor _signatureInterceptor;

        private void Init()
        {
            _apiKeyParamInterceptor = new ApiKeyParamInterceptor();
            _signatureParamInterceptor = new SignatureParamInterceptor();
            _timeParamInterceptor = new TimeParamInterceptor();
            _cmdInterceptor = new CmdInterceptor();

            _apiCategoryInterceptor = new ApiCategoryInterceptor();
            _formParamsInterceptor = new FormParamsInterceptor();
            _apiKeyInterceptor = new ApiKeyInterceptor();
            _signatureInterceptor = new SignatureInterceptor();

            _permitInterceptor = new PermitInterceptor();
            _resultInterceptor = new ResultInterceptor();


            _apiKeyParamInterceptor.SetSuccessor(_signatureParamInterceptor);
            _signatureParamInterceptor.SetSuccessor(_timeParamInterceptor);
            _timeParamInterceptor.SetSuccessor(_cmdInterceptor);
            _cmdInterceptor.SetSuccessor(_apiCategoryInterceptor);

            _apiCategoryInterceptor.SetSuccessor(_formParamsInterceptor);
            _formParamsInterceptor.SetSuccessor(_apiKeyInterceptor);
            _apiKeyInterceptor.SetSuccessor(_signatureInterceptor);
            _signatureInterceptor.SetSuccessor(_permitInterceptor);

            _permitInterceptor.SetSuccessor(_resultInterceptor);


        }

        public void Run(HttpContext @context)
        {
            _apiKeyParamInterceptor.RequestInterceptor(@context);
        }
    }
}
