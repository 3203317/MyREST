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

namespace Foreworld.Cmd.Sysmanage.Role
{
    [Implementation("unEmpowerRole", Description = "反授权角色", Version = "1.0.0.0")]
    class UnEmpowerRoleCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UnEmpowerRoleCmd));

        [Parameter("roleid", "角色名称ID", Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        [Parameter("modoptid", "模块操作ID", Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            #region SQL参数
            Dictionary<string, string> __parameters = @parameter.Parameters;
            SqlParameter[] __sps = new SqlParameter[2];

            __sps[0] = new SqlParameter("@tab_s_role_id", SqlDbType.Int);
            __sps[0].Value = __parameters["roleid"];

            __sps[1] = new SqlParameter("@tab_s_modopt_id", SqlDbType.Int);
            __sps[1].Value = __parameters["modoptid"];

#if DEBUG
            foreach (SqlParameter __sp_3 in __sps)
            {
                __logInfo.Msg = __sp_3 + ": " + __sp_3.SqlValue;
                _log.Debug(__logInfo);
            }

            __logInfo.Msg = Resource.UnEmpowerRoleCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region 执行SQL
            int __size = 0;

            try
            {
                __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Resource.UnEmpowerRoleCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Code = Resource.UnEmpowerRoleCmd_Err_Code;
                __logInfo.Msg = Resource.UnEmpowerRoleCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            if (__size == 0) return Util.ExceptionLog(Resource.UnEmpowerRoleCmd_Err);
            #endregion

            string __result = "{\"size\":\"" + __size + "\"}";

#if DEBUG
            __logInfo.Msg = __result;
            _log.Debug(__logInfo);
#endif
            __logInfo.Msg = string.Format(Resource.UnEmpowerRoleCmd, __sps[0].Value);
            _log.Info(__logInfo);

            return __result;
        }
    }
}
