#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using log4net;
using Newtonsoft.Json;

using Foreworld.Cmd;
using Foreworld.Db;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.Role
{
    [Implementation("deleteRole", Description = "删除角色", Version = "1.0.0.0")]
    class DeleteRoleCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DeleteRoleCmd));

        [Parameter("ids", "角色IDS", Required = true, Regexp = "^(\\d{1,10},?)*\\d{1,10}$", RegexpInfo = "支持逗号分割字符串")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            #region SQL参数
            Dictionary<string, string> __parameters = @parameter.Parameters;

            string[] __ids = __parameters["ids"].Split(',');

            List<SqlParameter> __sps = new List<SqlParameter>();

            string __appendSQL = string.Empty;

            for (int __i_3 = 0, __j_3 = __ids.Length; __i_3 < __j_3; __i_3++)
            {
                __appendSQL += ",@id" + __i_3;
                SqlParameter __sp_4 = new SqlParameter("@id" + __i_3, SqlDbType.Int, 10);
                __sp_4.Value = __ids[__i_3];
                __sps.Add(__sp_4);
            }

            string __sql = Resource.DeleteRoleCmd_Sql.Replace("@ids", __appendSQL);

#if DEBUG
            foreach (SqlParameter __sp_3 in __sps)
            {
                __logInfo.Msg = __sp_3 + ": " + __sp_3.SqlValue;
                _log.Debug(__logInfo);
            }

            __logInfo.Msg = __sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region 执行SQL
            int __size = 0;

            try
            {
                __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, __sql, __sps.ToArray());
            }
            catch (Exception @ex)
            {
                __logInfo.Code = Resource.DeleteRoleCmd_Err_Code;
                __logInfo.Msg = Resource.DeleteRoleCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            if (__size == 0) return Util.ExceptionLog(Resource.DeleteRoleCmd_Err);
            #endregion

            string __result = "{\"size\":\"" + __size + "\"}";

#if DEBUG
            __logInfo.Msg = __result;
            _log.Debug(__logInfo);
#endif
            __logInfo.Msg = string.Format(Resource.DeleteRoleCmd, __parameters["ids"]);
            _log.Info(__logInfo);

            return __result;
        }
    }
}
