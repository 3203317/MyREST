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
using Foreworld.Cmd.Sysmanage.Model;
using Foreworld.Cmd.Privilege.Model;
using Foreworld.Cmd.Privilege.Service;
using Foreworld.Cmd.Privilege.Service.Impl;

namespace Foreworld.Cmd.Sysmanage.Rest
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
        /// <returns></returns>
        [Resource(Public = true)]
        public ResultMapper Login(Parameter @parameter)
        {
            ResultMapper mapper = new ResultMapper();

            HttpContext httpContext = @parameter.HttpContext;
            HttpCookie cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            /* 验证码验证 */

            if (null == cookie || !httpContext.User.Identity.IsAuthenticated)
            {
                string dataStr_3 = GetDataStr(@parameter);
                LoginForm loginForm_3 = JavaScriptConvert.DeserializeObject<LoginForm>(dataStr_3);

                User user_3 = _userService.FindUserByName(loginForm_3.UserName);

                if (null == user_3)
                {
                    mapper.Msg = "查询用户失败";
                    return mapper;
                }
                else
                {
                    /* 验证密码 */
                    if (Utils.MD5.Encrypt(loginForm_3.UserPass).ToLower().Equals(user_3.UserPass.ToLower()))
                    {
                        user_3.UserPass = null;
                        /* 写入Session */
                        string userData_5 = JavaScriptConvert.SerializeObject(user_3);
                        int version_5 = Assembly.GetExecutingAssembly().GetName().Version.Major;
                        int expiration_5 = Convert.ToInt32(ConfigurationSettings.AppSettings["timeout"].Trim());

                        FormsAuthenticationTicket ticket_5 = new FormsAuthenticationTicket(version_5, user_3.UserName, DateTime.Now, DateTime.Now.AddHours(expiration_5), false, userData_5, FormsAuthentication.FormsCookiePath);
                        string encryptTicket_5 = FormsAuthentication.Encrypt(ticket_5);
                        HttpCookie cookie_5 = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket_5);
                        //cookie_4.Expires = ticket_4.Expiration;
                        /* 清除并写入Cookie */
                        httpContext.Response.Cookies.Clear();
                        httpContext.Response.Cookies.Add(cookie_5);

                        LogInfo logInfo_3 = @parameter.LogInfo;
                        logInfo_3.UserId = user_3.UserId;
                        logInfo_3.Msg = "登陆系统：/sysmanage/user/login.do";
                        _log.Info(logInfo_3);

                        mapper.Data = user_3;
                        mapper.Success = true;
                        return mapper;
                    }
                    else
                    {
                        mapper.Msg = "用户名或密码输入错误";
                        return mapper;
                    }
                }
            }
            else
            {
                mapper.Msg = "请不要重复登陆";
                mapper.Success = true;
                return mapper;
            }
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource]
        public ResultMapper Logout(Parameter @parameter)
        {
            HttpContext httpContext = @parameter.HttpContext;
            HttpCookie cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            User user = JavaScriptConvert.DeserializeObject<User>(ticket.UserData);

            LogInfo logInfo = @parameter.LogInfo;
            logInfo.UserId = user.UserId;
            logInfo.Msg = "退出系统：/sysmanage/user/logout";
            _log.Info(logInfo);

            /* 清除Session */
            FormsAuthentication.SignOut();

            ResultMapper mapper = new ResultMapper();
            mapper.Success = true;
            return mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource(Public = true)]
        public ResultMapper LoginUI(Parameter @parameter)
        {
            IContext vltCtx = new VelocityContext();
            vltCtx.Put("title", "后台管理登陆");

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate();
            htmlObj.Context = vltCtx;

            ResultMapper mapper = new ResultMapper();
            mapper.Data = htmlObj;
            mapper.Success = true;
            return mapper;
        }
    }
}
