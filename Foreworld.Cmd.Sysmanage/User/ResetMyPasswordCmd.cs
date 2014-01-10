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

namespace Foreworld.Cmd.Sysmanage.User
{
    [Implementation("resetMyPassword", Description = "重新设置我的密码", Version = "1.0.0.0")]
    class ResetMyPasswordCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ResetUserPasswordCmd));

        [Parameter("password", "新密码", Required = true, Regexp = "^[a-zA-Z0-9_]{6,16}$", RegexpInfo = "6-16个字符，支持英文大小写、数字、下划线，区分大小写")]
        [Parameter("opassword", "原始密码", Required = true, Regexp = "^[a-zA-Z0-9_]{6,16}$", RegexpInfo = "6-16个字符，支持英文大小写、数字、下划线，区分大小写")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: resetMyPassword [重新设置我的密码]");

            //验证老密码
            SqlParameter[] __sps = { new SqlParameter("@id", SqlDbType.Int), new SqlParameter("@password", SqlDbType.VarChar, 50) };
            __sps[0].Value = @parameter.UserInfo["id"];
            _log.Debug(__sps[0] + ": " + __sps[0].SqlValue);

            __sps[1].Value = Utils.MD5.Encrypt(@parameter.Parameters["opassword"]);
            _log.Debug(__sps[1] + ": " + __sps[1].SqlValue);

            _log.Debug(__sps);
            _log.Debug(Resource.LoginByIdCmd_Sql);

            DataSet __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.LoginByIdCmd_Sql, __sps);

            //操作异常
            if (__ds == null) throw new Exception(Resource.ResetMyPasswordCmd_Err);

            //老密码输入错误
            if (__ds.Tables[0].Rows.Count != 1)
            {
                __ds.Clear();
                __ds.Dispose();
                throw new Exception(Resource.ResetMyPasswordCmd_OldPassword_Err);
            }
            __ds.Clear();
            __ds.Dispose();


            /* 设置用户新密码 */
            __sps[1].Value = Utils.MD5.Encrypt(@parameter.Parameters["password"].ToString());
            _log.Debug(__sps[1] + ": " + __sps[1].SqlValue);

            _log.Debug(__sps);
            _log.Debug(Resource.ResetMyPasswordCmd_Sql);

            int __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Resource.ResetMyPasswordCmd_Sql, __sps);

            if (__size != 1) throw new Exception(Resource.ResetMyPasswordCmd_NewPassword_Err);

            string __result = "{'tip':'" + Resource.ResetMyPasswordCmd_Tip + "'}";

            _log.Debug(__result);

            return __result;
        }
    }
}
