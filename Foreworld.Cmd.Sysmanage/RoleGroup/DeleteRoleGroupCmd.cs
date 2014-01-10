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

namespace Foreworld.Cmd.Sysmanage.RoleGroup
{
    [Implementation("deleteRoleGroup", Description = "删除角色组", Version = "1.0.0.0")]
    class DeleteRoleGroupCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DeleteRoleGroupCmd));

        [Parameter("id", "角色组名称", Required = true, Regexp = "^[A-Z_]{2,20}$", RegexpInfo = "角色组名称为必填项")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            SqlParameter[] __sps = this.GetSqlParameters(__logInfo, @parameter.Parameters);

            int __size = this.DeleteRoleGroup(__logInfo, __sps);
            if (__size == 0) return Util.ExceptionLog(Resource.DeleteRoleGroupCmd_Err);

            string __result = "{\"size\":\"" + __size + "\"}";

#if DEBUG
            __logInfo.Msg = __result;
            _log.Debug(__logInfo);
#endif

            __logInfo.Msg = string.Format(Resource.DeleteRoleGroupCmd, __sps[0].SqlValue);
            _log.Info(__logInfo);

            return __result;
        }

        /// <summary>
        /// 组织SQL参数
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private SqlParameter[] GetSqlParameters(LogInfo @logInfo, Dictionary<string, string> @parameters)
        {
            SqlParameter[] __sps = new SqlParameter[1];

            __sps[0] = new SqlParameter("@id", SqlDbType.VarChar, 20);
            __sps[0].Value = @parameters["id"];

#if DEBUG
            @logInfo.Msg = __sps[0] + ": " + __sps[0].SqlValue;
            _log.Debug(@logInfo);

            @logInfo.Msg = Resource.DeleteRoleGroupCmd_Sql;
            _log.Debug(@logInfo);

            @logInfo.Msg = Resource.DeleteRoleGroupCmd_Sql2;
            _log.Debug(@logInfo);
#endif
            return __sps;
        }

        /// <summary>
        /// 数据库删除角色组
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="sps"></param>
        /// <returns></returns>
        private int DeleteRoleGroup(LogInfo @logInfo, SqlParameter[] @sps)
        {
            int __size = 0;

            using (SqlConnection __conn = new SqlConnection(ConnectionString))
            {
                __conn.Open();
                using (SqlTransaction __tran = __conn.BeginTransaction())
                {
                    try
                    {
                        __size = SqlHelper.ExecuteNonQuery(__tran, CommandType.Text, Resource.DeleteRoleGroupCmd_Sql, @sps);
                        __size = SqlHelper.ExecuteNonQuery(__tran, CommandType.Text, Resource.DeleteRoleGroupCmd_Sql2, @sps) + __size;
                        __tran.Commit();
                    }
                    catch (Exception @ex)
                    {
                        __tran.Rollback();
                        @logInfo.Code = Resource.DeleteCodeTypeCmd_Err_Code;
                        @logInfo.Msg = Resource.DeleteCodeTypeCmd_Err;
                        _log.Error(@logInfo, @ex);
                    }
                    finally
                    {
                        __conn.Close();
                    }
                }
            }

            return __size;
        }
    }
}