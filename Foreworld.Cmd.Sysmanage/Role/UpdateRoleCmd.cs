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

namespace Foreworld.Cmd.Sysmanage.Role
{
    [Implementation("updateRole", Description = "更新角色", Version = "1.0.0.0")]
    class UpdateRoleCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateRoleCmd));

        [Parameter("id", "角色ID", Required = true, Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        [Parameter("rolename", "角色名称", Required = true, Regexp = "^[\u4E00-\u9FA5a-zA-Z0-9_]{2,10}$", RegexpInfo = "2-10个字符，支持中文，英文大小写、数字、下划线")]
        [Parameter("roledesc", "角色描述")]
        [Parameter("startime", "启用日期起", Required = true, Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字", DefaultValue = "0")]
        [Parameter("endtime", "启用日期止", Required = true, Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字", DefaultValue = "31507200")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            #region SQL参数
            Dictionary<string, string> __parameters = @parameter.Parameters;

            int __startime = Convert.ToInt32(__parameters["startime"]);
            int __endtime = Convert.ToInt32(__parameters["endtime"]);

            if (__startime >= __endtime) return Util.ExceptionLog(Resource.UpdateRoleCmd_Err);

            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@id", SqlDbType.Int);
            __sp.Value = __parameters["id"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@rolename", SqlDbType.NVarChar, 20);
            __sp.Value = __parameters["rolename"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@roledesc", SqlDbType.NVarChar, 100);
            __sp.Value = __parameters["roledesc"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@startime", SqlDbType.Int, 10);
            __sp.Value = __startime;
            __sps.Add(__sp);

            __sp = new SqlParameter("@endtime", SqlDbType.Int, 10);
            __sp.Value = __endtime;
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

            __logInfo.Msg = Resource.UpdateRoleCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region DataSet
            DataSet __ds = null;

            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.UpdateRoleCmd_Sql, __sps.ToArray());
            }
            catch (Exception @ex)
            {
                __logInfo.Code = Resource.UpdateRoleCmd_Err_Code;
                __logInfo.Msg = Resource.UpdateRoleCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            //找不到记录
            if (__ds == null) return Util.ExceptionLog(Resource.UpdateRoleCmd_Err);

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count == 0)
            {
                __ds.Clear();
                __ds.Dispose();
                return Util.ExceptionLog(Resource.UpdateRoleCmd_Err);
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
            __jw.WriteValue("role");

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
            __logInfo.Msg = string.Format(Resource.UpdateRoleCmd, __parameters["id"]);
            _log.Info(__logInfo);

            return __result;
        }
    }
}

