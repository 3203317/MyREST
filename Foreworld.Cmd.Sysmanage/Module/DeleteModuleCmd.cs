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
using Foreworld.Utils;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.Module
{
    [Implementation("deleteModule", Description = "删除模块", Version = "1.0.0.0")]
    class DeleteModuleCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DeleteModuleCmd));

        [Parameter("ids", "模块IDS", Required = true, Regexp = "^(\\d{1,10},?)*\\d{1,10}$", RegexpInfo = "支持逗号分割字符串")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

#if DEBUG
            __logInfo.Msg = "[删除模块]";
            _log.Debug(__logInfo);
#endif

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

            string __sql = Resource.DeleteModuleCmd_Sql.Replace("@ids", __appendSQL);

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
                __logInfo.Msg = Resource.DeleteModuleCmd_Err;
                __logInfo.Code = Resource.DeleteModuleCmd_Err_Code;
                _log.Error(__logInfo, @ex);
            }

            if (__size == 0) return Util.ExceptionLog(Resource.DeleteModuleCmd_Err);
            #endregion

            string __result = "{'size':'" + __size + "'}";

#if DEBUG
            __logInfo.Msg = __result;
            _log.Debug(__logInfo);
#endif

            __logInfo.Msg = string.Format(Resource.DeleteModuleCmd, __parameters["ids"]);
            _log.Info(__logInfo);

            return __result;
        }
    }
}
