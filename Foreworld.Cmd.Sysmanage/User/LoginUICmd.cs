#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

using log4net;
using Newtonsoft.Json;

using Foreworld.Cmd;
using Foreworld.Db;
using Foreworld.Utils;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.User
{
    [Implementation("/user/loginUI", Description = "用户登陆UI")]
    public class LoginUICmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LoginUICmd));

        public override AccessLevel Access
        {
            get { return AccessLevel.PUBLIC; }
        }

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = new LogInfo(0);

            HttpContext __httpCtx = HttpContext.Current;

            if (null != __httpCtx)
            {
                HttpRequest __request = __httpCtx.Request;

                string __data = __request.Form["data"];

                //IContext __context = new VelocityContext();
                //__context.Put("Title", "用户登陆");

                //HttpContext.Current.Items.Add("context", __context);
            }



            return null;
        }
    }
}
