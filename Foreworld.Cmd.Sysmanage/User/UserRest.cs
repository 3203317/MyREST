using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Reflection;

using log4net;
using Newtonsoft.Json;

using NVelocity;
using NVelocity.Context;

using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.User
{
    public class UserRest : BaseRest
    {
        private UserService _userService;

        public UserRest()
        {
            _userService = new UserServiceImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(UserRest));

        /// <summary>
        /// 后台管理登陆
        /// 描述：
        /// 1、判断是否已经登陆过，如果已经登陆过，则直接返回成功
        /// 2、验证用户名和密码，成功后写入Session
        /// </summary>
        /// <param name="parameter"></param>
        public void Login(Parameter @parameter)
        {
            ResultMapper __mapper = null;
            HttpCookie __cookie = @parameter.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];


            LogInfo __logInfo_4 = @parameter.LogInfo;
            __logInfo_4.Msg = (__cookie == null).ToString();
            _log.Info(__logInfo_4);
            __logInfo_4.Msg = (FormsAuthentication.FormsCookieName);
            _log.Info(__logInfo_4);

            if (__cookie == null || !@parameter.HttpContext.User.Identity.IsAuthenticated)
            {
                string __dataStr_3 = GetData(@parameter);
                LoginForm __loginForm_3 = JavaScriptConvert.DeserializeObject<LoginForm>(__dataStr_3);

                __mapper = _userService.Login(__loginForm_3);


                __logInfo_4.Msg = (ResultMapper.StatusType.SUCCESS == __mapper.Status).ToString();
                _log.Info(__logInfo_4);

                /* 写入Session */
                if (ResultMapper.StatusType.SUCCESS == __mapper.Status)
                {
                    string __userData_4 = JavaScriptConvert.SerializeObject(__mapper.Data);
                    __logInfo_4.Msg = __userData_4;
                    _log.Info(__logInfo_4);

                    string __username_4 = __loginForm_3.UserName;
                    int __version_4 = Assembly.GetExecutingAssembly().GetName().Version.Major;
                    int __expiration_4 = Convert.ToInt32(ConfigurationSettings.AppSettings["timeout"].Trim());

                    FormsAuthenticationTicket __ticket_4 = new FormsAuthenticationTicket(__version_4, __username_4, DateTime.Now, DateTime.Now.AddHours(__expiration_4), false, __userData_4, "/");
                    string __encryptTicket_4 = FormsAuthentication.Encrypt(__ticket_4);
                    HttpCookie __cookie_4 = new HttpCookie(FormsAuthentication.FormsCookieName, __encryptTicket_4);
                    //__cookie_4.Expires = __ticket_4.Expiration;

                    @parameter.HttpContext.Response.Cookies.Add(__cookie_4);

                    __logInfo_4.Msg = (FormsAuthentication.FormsCookieName);
                    _log.Info(__logInfo_4);

                }
                @parameter.ResultMapper = __mapper;

                LogInfo __logInfo_3 = @parameter.LogInfo;
                __logInfo_3.Msg = "用户登陆成功";
                _log.Info(__logInfo_3);
            }
            else
            {
                __mapper = new ResultMapper();
                __mapper.Status = ResultMapper.StatusType.SUCCESS;
                @parameter.ResultMapper = __mapper;
            }
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="parameter"></param>
        public void Logout(Parameter @parameter)
        {
            @parameter.ResultMapper = _userService.Logout();
            /* 清除Session */
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void LoginUI(Parameter @parameter)
        {
            IContext __ctx = new VelocityContext();
            __ctx.Put("Title", "后台管理登陆");

            ResultMapper __mapper = new ResultMapper();
            __mapper.Data = __ctx;
            __mapper.Msg = TplRes.LoginUI;
            __mapper.Status = ResultMapper.StatusType.SUCCESS;
            @parameter.ResultMapper = __mapper;
        }
    }
}
