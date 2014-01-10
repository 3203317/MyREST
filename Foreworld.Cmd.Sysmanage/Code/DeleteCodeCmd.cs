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

namespace Foreworld.Cmd.Sysmanage.Code
{
    [Implementation("deleteCode", Description = "删除代码", Version = "1.0.0.0")]
    class DeleteCodeCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DeleteCodeCmd));

        [Parameter("code", "编码", Required = true, Regexp = "^[A-Z1-9][A-Z0-9_]{0,19}$", RegexpInfo = "必填项")]
        [Parameter("codetypeid", "代码类型名称", Required = true, Regexp = "^[A-Z_]{2,20}$", RegexpInfo = "必填项")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            #region SQL参数
            Dictionary<string, string> __parameters = @parameter.Parameters;

            SqlParameter[] __sps = new SqlParameter[2];

            __sps[0] = new SqlParameter("@code", SqlDbType.VarChar, 20);
            __sps[0].Value = __parameters["code"];

            __sps[1] = new SqlParameter("@tab_p_codetype_id", SqlDbType.VarChar, 20);
            __sps[1].Value = __parameters["codetypeid"];

#if DEBUG
            foreach (SqlParameter __sp_3 in __sps)
            {
                __logInfo.Msg = __sp_3 + ": " + __sp_3.SqlValue;
                _log.Debug(__logInfo);
            }

            __logInfo.Msg = Resource.DeleteCodeCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region 执行SQL
            int __size = 0;

            try
            {
                __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Resource.DeleteCodeCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Code = Resource.DeleteCodeCmd_Err_Code;
                __logInfo.Msg = Resource.DeleteCodeCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            if (__size == 0) return Util.ExceptionLog(Resource.DeleteCodeCmd_Err);
            #endregion

            string __result = "{\"size\":\"" + __size + "\"}";

#if DEBUG
            __logInfo.Msg = __result;
            _log.Debug(__logInfo);
#endif

            __logInfo.Msg = string.Format(Resource.DeleteCodeCmd, __sps[0].Value + "," + __sps[1].Value);
            _log.Info(__logInfo);

            return __result;
        }
    }
}
