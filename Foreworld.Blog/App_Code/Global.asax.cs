using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using log4net;

using Foreworld.Log;

namespace Foreworld.Blog
{
    /// <summary>
    ///Global 的摘要说明
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private System.ComponentModel.IContainer components = null;

        public Global()
        {
            InitializeComponent();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(Global));
        private static LogInfo _logInfo = new LogInfo();

        void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            _logInfo.Msg = "主程序启动...";
            _log.Info(_logInfo);

            //在应用程序启动时运行的代码
            Application.Lock();
            Application.UnLock();
        }

        void Application_End(object sender, EventArgs e)
        {
            //在应用程序关闭时运行的代码
            _logInfo.Msg = "主程序停止...";
            _log.Info(_logInfo);
        }

        void Application_Error(object sender, EventArgs e)
        {
            //在出现未处理的错误时运行的代码
            Exception __ex = Server.GetLastError();
            if (__ex is HttpException)
            {
                int __httpCode_3 = ((HttpException)__ex).GetHttpCode();

                if (404 != __httpCode_3)
                {
                    _logInfo.Msg = "HTTP错误：" + __httpCode_3;
                    _log.Error(_logInfo, __ex);
                }
            }
            else
            {
                _logInfo.Msg = "运行时错误...";
                _log.Error(_logInfo, __ex);
                //Server.ClearError();
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            //在新会话启动时运行的代码
        }

        void Session_End(object sender, EventArgs e)
        {
            //在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式 
            //设置为 StateServer 或 SQLServer，则不会引发该事件。
        }

        #region Web 窗体组件
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }
        #endregion

    }
}