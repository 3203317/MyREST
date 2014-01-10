﻿#define DEBUG
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
    [Implementation("updateModule", Description = "更新模块", Version = "1.0.0.0")]
    class UpdateModuleCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateModuleCmd));

        [Parameter("id", "模块ID", Required = true, Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        [Parameter("modulename", "模块名称", Required = true, Regexp = "^.{2,20}$", RegexpInfo = "长度为2-20位任意字符")]
        [Parameter("icon", "图标")]
        [Parameter("href", "模块地址")]
        [Parameter("sort", "排序", Regexp = "^\\d{1,3}$", RegexpInfo = "支持数字", DefaultValue = "1")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

#if DEBUG
            __logInfo.Msg = "[更新模块]";
            _log.Debug(__logInfo);
#endif

            #region SQL参数
            Dictionary<string, string> __parameters = @parameter.Parameters;

            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@id", SqlDbType.Int, 10);
            __sp.Value = __parameters["id"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@modulename", SqlDbType.NVarChar, 20);
            __sp.Value = __parameters["modulename"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@href", SqlDbType.VarChar, 100);
            __sp.Value = __parameters["href"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@icon", SqlDbType.VarChar, 100);
            __sp.Value = __parameters["icon"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@sort", SqlDbType.Int, 3);
            __sp.Value = __parameters["sort"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@editime", SqlDbType.DateTime);
            __sp.Value = DateTime.Now;
            __sps.Add(__sp);

            __sp = new SqlParameter("@ed_s_user_id", SqlDbType.Int);
            __sp.Value = @parameter.UserInfo["id"];
            __sps.Add(__sp);

#if DEBUG
            foreach (SqlParameter __sp_3 in __sps)
            {
                __logInfo.Msg = __sp_3 + ": " + __sp_3.SqlValue;
                _log.Debug(__logInfo);
            }

            __logInfo.Msg = Resource.UpdateModuleCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region DataSet
            DataSet __ds = null;
            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.UpdateModuleCmd_Sql, __sps.ToArray());
            }
            catch (Exception @ex)
            {
                __logInfo.Msg = Resource.UpdateModuleCmd_Err;
                __logInfo.Code = Resource.UpdateModuleCmd_Err_Code;
                _log.Error(__logInfo, @ex);
            }

            //找不到记录
            if (__ds == null) return Util.ExceptionLog(Resource.UpdateModuleCmd_Err);


            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count == 0)
            {
                __ds.Clear();
                __ds.Dispose();
                return Util.ExceptionLog(Resource.UpdateModuleCmd_Err);
            }
            #endregion

            #region JSON对象
            //JSON对象创建
            StringWriter __sw = new StringWriter();
            JsonWriter __jw = new JsonWriter(__sw);

#if DEBUG
            __jw.Formatting = Formatting.Indented;
#endif

            __jw.WriteStartObject();
            __jw.WritePropertyName("items");
            __jw.WriteStartArray();

            __jw.WriteStartObject();

            DataRow __row = __rows[0];
            DataColumnCollection __columns = __dt.Columns;

            for (int __i_3 = 0, __j_3 = __columns.Count; __i_3 < __j_3; __i_3++)
            {
                __jw.WritePropertyName(__columns[__i_3].ToString());
                __jw.WriteValue(__row[__i_3].ToString());
            }

            __jw.WritePropertyName("type");
            __jw.WriteValue("module");

            __jw.WriteEndObject();

            __jw.WriteEndArray();
            __jw.WriteEndObject();

            __jw.Flush();

            string __result = __sw.GetStringBuilder().ToString();

            __ds.Clear();
            __ds.Dispose();
            __jw.Close();
            __sw.Close();
            __sw.Dispose();
            #endregion

#if DEBUG
            __logInfo.Msg = __result;
            _log.Debug(__logInfo);
#endif

            __logInfo.Msg = string.Format(Resource.UpdateModuleCmd, __parameters["id"]);
            _log.Info(__logInfo);

            return __result;
        }
    }
}
