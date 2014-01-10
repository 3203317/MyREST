using System;
using System.Web;

using log4net;

using Foreworld.Rest;
using Foreworld.Log;

namespace Foreworld.Rest.Api
{
    class ApInterceptorManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ApInterceptorManager));
        private static LogInfo _logInfo = new LogInfo();

        private ApInterceptorManager()
        {
            _logInfo.Msg = "ApInterceptorManager初始化...";
            _log.Info(_logInfo);
            Init();
        }

        private static volatile ApInterceptorManager _instance;
        private static object _syncRoot = new Object();

        public static ApInterceptorManager INSTANCE
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null) _instance = new ApInterceptorManager();
                    }
                }
                return _instance;
            }
        }

        private Interceptor _timeParamInterceptor;

        private Interceptor _formParamsInterceptor;
        private Interceptor _sessionInterceptor;
        private Interceptor _resultInterceptor;
        private Interceptor _loginOutInterceptor;

        private Interceptor _apiCategoryInterceptor;
        private Interceptor _cmdInterceptor;
        private Interceptor _cookieInterceptor;
        private Interceptor _permitInterceptor;

        private void Init()
        {
            _timeParamInterceptor = new TimeParamInterceptor();

            _cmdInterceptor = new CmdInterceptor();
            _apiCategoryInterceptor = new ApiCategoryInterceptor();
            _cookieInterceptor = new CookieInterceptor();

            _formParamsInterceptor = new FormParamsInterceptor();
            _sessionInterceptor = new SessionInterceptor();
            _permitInterceptor = new PermitInterceptor();
            _resultInterceptor = new ResultInterceptor();
            _loginOutInterceptor = new LoginOutInterceptor();


            _timeParamInterceptor.SetSuccessor(_cmdInterceptor);
            _cmdInterceptor.SetSuccessor(_apiCategoryInterceptor);

            _apiCategoryInterceptor.SetSuccessor(_cookieInterceptor);

            _cookieInterceptor.SetSuccessor(_formParamsInterceptor);

            _formParamsInterceptor.SetSuccessor(_sessionInterceptor);

            _sessionInterceptor.SetSuccessor(_permitInterceptor);
            _permitInterceptor.SetSuccessor(_resultInterceptor);
            _resultInterceptor.SetSuccessor(_loginOutInterceptor);

        }

        public void Run(HttpContext @context)
        {
            _timeParamInterceptor.RequestInterceptor(@context);
        }
    }
}
