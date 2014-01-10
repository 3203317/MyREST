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
    [Implementation("resetUserPassword", Description = "重置用户密码", Version = "1.0.0.0")]
    class ResetUserPasswordCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ResetUserPasswordCmd));

        [Parameter("userid", "用户ID", Required = true, Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        [Parameter("password", "新密码", Required = true, Regexp = "^[a-zA-Z0-9_]{6,16}$", RegexpInfo = "6-16个字符，支持英文大小写、数字、下划线，区分大小写")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: resetUserPassword [重置用户密码]");

            //验证老密码
            SqlParameter[] __sps = { new SqlParameter("@id", SqlDbType.Int), new SqlParameter("@password", SqlDbType.VarChar, 50) };
            __sps[0].Value = @parameter.Parameters["userid"];
            _log.Debug(__sps[0] + ": " + __sps[0].SqlValue);

            __sps[1].Value = Utils.MD5.Encrypt(@parameter.Parameters["password"].ToString());
            _log.Debug(__sps[1] + ": " + __sps[1].SqlValue);

            _log.Debug(__sps);
            _log.Debug(Resource.ResetUserPasswordCmd_Sql);

            int __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Resource.ResetUserPasswordCmd_Sql, __sps);

            if (__size != 1) throw new Exception(Resource.ResetUserPasswordCmd_Err);

            string __result = "{'tip':'" + Resource.ResetUserPasswordCmd_Tip + "'}";

            _log.Debug(__result);

            return __result;
        }
    }
}
