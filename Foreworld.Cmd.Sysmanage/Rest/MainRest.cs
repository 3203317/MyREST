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
using Foreworld.Cmd.Privilege.Model;
using Foreworld.Cmd.Privilege.Service;
using Foreworld.Cmd.Privilege.Service.Impl;

namespace Foreworld.Cmd.Sysmanage.Rest
{
    using Module = Foreworld.Cmd.Privilege.Model.Module;

    public class MainRest : BaseRest
    {
        private UserService _userService;

        public MainRest()
        {
            _userService = new UserServiceImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(MainRest));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource]
        public ResultMapper IndexUI(Parameter @parameter)
        {
            HttpContext httpContext = @parameter.HttpContext;
            HttpCookie cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            HttpResponse response = httpContext.Response;

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            User user = JavaScriptConvert.DeserializeObject<User>(ticket.UserData);

            List<Module> list = _userService.GetMenuTreeById(user.UserId);

            if (0 == list.Count)
            {
                response.Redirect("../user/loginUI.html");
                response.End();
                return null;
            }

            IContext vltCtx = new VelocityContext();
            vltCtx.Put("title", "核心平台");
            vltCtx.Put("menuTree", JavaScriptConvert.SerializeObject(list));

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate();
            htmlObj.Context = vltCtx;

            ResultMapper mapper = new ResultMapper();
            mapper.Data = htmlObj;
            mapper.Success = true;
            return mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource]
        public ResultMapper WelcomeUI(Parameter @parameter)
        {
            IContext vltCtx = new VelocityContext();
            vltCtx.Put("title", "欢迎页");

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
