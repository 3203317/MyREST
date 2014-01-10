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

namespace Foreworld.Cmd.Sysmanage.CodeType
{
    [Implementation("deleteCodeType", Description = "删除代码类型", Version = "1.0.0.0")]
    class DeleteCodeTypeCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DeleteCodeTypeCmd));

        [Parameter("id", "代码类型名称", Required = true, Regexp = "^[A-Z_]{2,20}$", RegexpInfo = "代码类型名称为必填项")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            #region SQL参数
            SqlParameter[] __sps = new SqlParameter[1];

            __sps[0] = new SqlParameter("@id", SqlDbType.VarChar, 20);
            __sps[0].Value = @parameter.Parameters["id"];

#if DEBUG
            __logInfo.Msg = __sps[0] + ": " + __sps[0].SqlValue;
            _log.Debug(__logInfo);

            __logInfo.Msg = Resource.DeleteCodeTypeCmd_Sql;
            _log.Debug(__logInfo);

            __logInfo.Msg = Resource.DeleteCodeTypeCmd_Sql2;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region 执行SQL
            int __size = 0;

            using (SqlConnection __conn = new SqlConnection(ConnectionString))
            {
                __conn.Open();
                using (SqlTransaction __tran = __conn.BeginTransaction())
                {
                    try
                    {
                        __size = SqlHelper.ExecuteNonQuery(__tran, CommandType.Text, Resource.DeleteCodeTypeCmd_Sql, __sps);
                        __size = SqlHelper.ExecuteNonQuery(__tran, CommandType.Text, Resource.DeleteCodeTypeCmd_Sql2, __sps) + __size;
                        __tran.Commit();
                    }
                    catch (Exception @ex)
                    {
                        __tran.Rollback();
                        __logInfo.Code = Resource.DeleteCodeTypeCmd_Err_Code;
                        __logInfo.Msg = Resource.DeleteCodeTypeCmd_Err;
                        _log.Error(__logInfo, @ex);
                    }
                    finally
                    {
                        __conn.Close();
                    }
                }
            }

            if (__size == 0) return Util.ExceptionLog(Resource.DeleteCodeTypeCmd_Err);
            #endregion

            string __result = "{\"size\":\"" + __size + "\"}";

#if DEBUG
            __logInfo.Msg = __result;
            _log.Debug(__logInfo);
#endif

            __logInfo.Msg = string.Format(Resource.DeleteCodeTypeCmd, __sps[0].SqlValue);
            _log.Info(__logInfo);

            return __result;
        }
    }
}
