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
    [Implementation("/user/loginBack", Description = "后台管理登陆")]
    public class LoginBackCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LoginBackCmd));

        public override AccessLevel Access
        {
            get { return AccessLevel.PUBLIC; }
        }

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = new LogInfo(0);

            HttpContext __context = @parameter.HttpContext;

            string __data = GetData(__context);

            ResultMapper __mapper = @parameter.ResultMapper;
            __mapper.Status = ResultMapper.StatusType.SUCCESS;
            return null;
        }
    }
}
