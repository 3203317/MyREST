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

namespace Foreworld.Cmd.Build.DataSource
{
    [Implementation("sds", Description = "安全数据源服务", Version = "1.0.0.0")]
    class SDSCmd : AbstractBaseCmd
    {
        private LogInfo _logInfo;

        private static readonly ILog _log = LogManager.GetLogger(typeof(SDSCmd));

        [Parameter("name", "数据源名称", Required = true, Regexp = "^[A-Z0-9]{32}$", RegexpInfo = "必填项")]
        [Parameter("current", "当前页", Regexp = "^[1-9]\\d{0,8}$", DefaultValue = "1", RegexpInfo = "支持数字")]
        [Parameter("pagesize", "每页大小", Regexp = "^[1-9]\\d{0,2}$", DefaultValue = "20", RegexpInfo = "支持数字")]
        [Parameter("params", "参数序列化", Regexp = "^(?!.*(select|update|insert|delete|declare|@|exec|dbcc|alter|drop|create|backup|if|else|end|set|open|close|use|begin|retun|as|go|exists|;)).*$", RegexpInfo = "选填项")]
        public override object Execute(Parameter @parameter)
        {
            _logInfo = @parameter.LogInfo;
#if DEBUG
            _logInfo.Msg = "[安全数据源服务]";
            _log.Debug(_logInfo);
#endif
            Dictionary<string, string> __params = @parameter.Parameters;
            /* 组装SQL语句 */
            string __sql = GetDSQL(__params["name"]);
            __sql = __sql.Replace("@where", __params["params"]).Replace("@current", __params["current"]).Replace("@pagesize", __params["pagesize"]);
#if DEBUG
            _logInfo.Msg = __sql;
            _log.Debug(_logInfo);
#endif

            DataSet __ds = null;

            #region DataSet
            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, __sql);
            }
            catch (Exception @ex)
            {
                _logInfo.Code = Resource.SDSCmd_Err_1;
                _log.Error(_logInfo, @ex);
            }
            #endregion

            #region 判断ds是否为空
            if (__ds == null) return Util.ExceptionLog(Resource.SDSCmd_Err_1);
            #endregion

#if DEBUG
            _logInfo.Msg = __ds.GetXml();
            _log.Debug(_logInfo);
#endif
            return __ds;
        }

        /// <summary>
        /// 获取数据源的SQL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetDSQL(string @id)
        {
            string __sql = string.Empty;

            __sql = GetDSQLFromCache(@id);

            if (__sql.Length > 0) return __sql;

            __sql = GetDSQLFromDB(@id);

            WriteCache(@id, __sql);

            return __sql;
        }

        /// <summary>
        /// 从缓存中获取数据源的SQL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetDSQLFromCache(string @id)
        {
            string __sql = string.Empty;
            object __ds = HttpContext.Current.Cache[@id];
            if (__ds != null)
            {
                __sql = __ds.ToString();
#if DEBUG
                _logInfo.Msg = "读取缓存: " + @id;
                _log.Debug(_logInfo);
#endif
            }
            return __sql;
        }

        /// <summary>
        /// 从数据库中获取数据源的SQL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetDSQLFromDB(string @id)
        {
            List<SqlParameter> __sps = new List<SqlParameter>();
            SqlParameter __sp_3 = new SqlParameter("@id", SqlDbType.VarChar, 32);
            __sp_3.Value = @id;
            __sps.Add(__sp_3);

            DataSet __ds = null;

            #region DataSet
            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.SDSCmd_Sql, __sps.ToArray());
            }
            catch (Exception @ex)
            {
#if DEBUG
                _logInfo.Msg = Resource.SDSCmd_Err_2;
                _log.Error(_logInfo, @ex);
#endif
            }
            #endregion

            string __sql = string.Empty;

            if (__ds == null) return __sql;

            /* 获取记录总数 */
            int __rowsCount = __ds.Tables[0].Rows.Count;

            if (__rowsCount == 1)
            {
                __sql = __ds.Tables[0].Rows[0][1].ToString();
#if DEBUG
                _logInfo.Msg = __sql;
                _log.Debug(_logInfo);
#endif
            }

            /* 数据源类型 */
            string __dstype = __ds.Tables[0].Rows[0][0].ToString();

            __ds.Clear();
            __ds.Dispose();

            if (__dstype == "1")
                __sql = Resource.SDSCmd_Sql_1 + __sql + Resource.SDSCmd_Sql_2;
            else
                __sql += Resource.SDSCmd_Sql_3;

            return __sql;
        }

        /// <summary>
        /// 数据源信息写入缓存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sql"></param>
        private void WriteCache(string @id, string @sql)
        {
            HttpContext.Current.Cache.Insert(@id, @sql);
#if DEBUG
            _logInfo.Msg = "写入缓存: " + @id;
            _log.Debug(_logInfo);
#endif
        }
    }
}
