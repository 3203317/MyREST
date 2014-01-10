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
    [Implementation("listModules", Description = "模块列表", Version = "1.0.0.0")]
    class ListModulesCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListModulesCmd));

        [Parameter("p_id", "父ID", Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;
#if DEBUG
            __logInfo.Msg = "[模块列表]";
            _log.Debug(__logInfo);
#endif

            #region SQL参数
            Dictionary<string, string> __parameters = @parameter.Parameters;

            List<SqlParameter> __sps = new List<SqlParameter>();

            string __appendSQL = String.Empty;

            if (__parameters.ContainsKey("p_id") && __parameters["p_id"].Length > 0)
            {
                __appendSQL += " and a.p_id=@p_id";
                SqlParameter __sp_3 = new SqlParameter("@p_id", SqlDbType.Int, 10);
                __sp_3.Value = __parameters["p_id"];
                __sps.Add(__sp_3);
            }

            string __sql = Resource.ListModulesCmd_Sql.Replace("@appendSQL", __appendSQL);

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

            DataSet __ds = null;

            #region DataSet
            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, __sql, __sps.ToArray());
            }
            catch (Exception @ex)
            {
                __logInfo.Msg = Resource.ListModulesCmd_Err;
                __logInfo.Code = Resource.ListModulesCmd_Err_Code;
                _log.Error(__logInfo, @ex);
            }
            #endregion

            #region 判断ds是否为空
            if (__ds == null) return Util.ExceptionLog(Resource.ListModulesCmd_Err);
            #endregion

#if DEBUG
            __logInfo.Msg = __ds.GetXml();
            _log.Debug(__logInfo);
#endif

            return __ds;
        }
    }
}
