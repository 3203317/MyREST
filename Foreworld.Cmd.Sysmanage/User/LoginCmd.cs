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
    [Implementation("/user/login", Description = "用户登陆")]
    public class LoginCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LoginCmd));

        public override AccessLevel Access
        {
            get { return AccessLevel.PUBLIC; }
        }

        [Parameter("username", "用户名", Required = true, Regexp = "^[\u4E00-\u9FA5a-zA-Z0-9_]{2,10}$", RegexpInfo = "2-10个字符，支持中文，英文大小写、数字、下划线")]
        [Parameter("password", "密码", Required = true, Regexp = "^[a-zA-Z0-9_]{6,16}$", RegexpInfo = "6-16个字符，支持英文大小写、数字、下划线，区分大小写")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = new LogInfo(0);

            //            #region SQL参数
            //            Dictionary<string, string> __parameters = @parameter.Parameters;

            //            SqlParameter[] __sps = { new SqlParameter("@username", SqlDbType.NVarChar, 10), new SqlParameter("@password", SqlDbType.VarChar, 50) };

            //            __sps[0].Value = __parameters["username"];
            //            __sps[1].Value = Utils.MD5.Encrypt(__parameters["password"]);

            //#if DEBUG
            //            foreach (SqlParameter __sp_3 in __sps)
            //            {
            //                __logInfo.Msg = __sp_3 + ": " + __sp_3.SqlValue;
            //                _log.Debug(__logInfo);
            //            }

            //            __logInfo.Msg = Resource.LoginCmd_Sql;
            //            _log.Debug(__logInfo);
            //#endif
            //            #endregion

            //            DataSet __ds = null;

            //            #region DataSet
            //            try
            //            {
            //                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.LoginCmd_Sql, __sps);
            //            }
            //            catch (Exception @ex)
            //            {
            //                __logInfo.Code = Resource.LoginCmd_Err_Code;
            //                __logInfo.Msg = Resource.LoginCmd_Err;
            //                _log.Error(__logInfo, @ex);
            //            }
            //            #endregion

            //            #region 判断ds是否为空
            //            if (__ds == null) return Util.ExceptionLog(Resource.LoginCmd_Err);
            //            #endregion

            //            #region 判断记录总数是否为1
            //            /* 获取记录总数 */
            //            int __rowsCount = __ds.Tables[0].Rows.Count;

            //            if (__rowsCount != 1)
            //            {
            //                __ds.Clear();
            //                __ds.Dispose();
            //                return Util.ExceptionLog(Resource.LoginCmd_Err);
            //            }
            //            #endregion

            //#if DEBUG
            //            __logInfo.Msg = __ds.GetXml();
            //            _log.Debug(__logInfo);
            //#endif
            //            __logInfo.Msg = Resource.LoginCmd;
            //            _log.Info(__logInfo);





            /******/
            HttpContext __context = @parameter.HttpContext;

            string __data = GetData(__context);





            //LoginForm __form = JavaScriptConvert.DeserializeObject<LoginForm>(__data);



            ResultMapper __mapper = @parameter.ResultMapper;
            __mapper.Status = ResultMapper.StatusType.SUCCESS;
            return null;
        }
    }
}
