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
    [Implementation("addRoleGroup", Description = "添加角色组", Version = "1.0.0.0")]
    class AddRoleGroupCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AddRoleGroupCmd));

        [Parameter("id", "角色组名称", Required = true, Regexp = "^[A-Z_]{2,20}$", RegexpInfo = "角色组名称为必填项")]
        [Parameter("rolegroupdesc", "角色组描述")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            SqlParameter[] __sps = this.GetSqlParameters(__logInfo, @parameter.Parameters, @parameter.UserInfo["id"]);

            #region DataSet
            DataSet __ds = null;

            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.AddRoleGroupCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Code = Resource.AddRoleGroupCmd_Err_Code;
                __logInfo.Msg = Resource.AddRoleGroupCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            //找不到新记录
            if (__ds == null) return Util.ExceptionLog(Resource.AddRoleGroupCmd_Err);

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count == 0)
            {
                __ds.Clear();
                __ds.Dispose();
                return Util.ExceptionLog(Resource.AddRoleGroupCmd_Err);
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
            __jw.WriteValue("rolegroup");

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

            __logInfo.Msg = string.Format(Resource.AddRoleGroupCmd, @parameter.Parameters["id"]);
            _log.Info(__logInfo);

            return __result;
        }

        #region SQL参数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="parameters"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private SqlParameter[] GetSqlParameters(LogInfo logInfo, Dictionary<string, string> @parameters, object @userId)
        {
            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@id", SqlDbType.VarChar, 20);
            __sp.Value = @parameters["id"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@rolegroupdesc", SqlDbType.NVarChar, 100);
            __sp.Value = @parameters["rolegroupdesc"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@addtime", SqlDbType.DateTime);
            __sp.Value = DateTime.Now;
            __sps.Add(__sp);

            __sp = new SqlParameter("@ad_s_user_id", SqlDbType.Int);
            __sp.Value = @userId;
            __sps.Add(__sp);

            __sp = new SqlParameter("@isenable", SqlDbType.Int);
            __sp.Value = 1;
            __sps.Add(__sp);

            __sp = new SqlParameter("@invalid", SqlDbType.Int);
            __sp.Value = 1;
            __sps.Add(__sp);

#if DEBUG
            foreach (SqlParameter __sp_3 in __sps)
            {
                logInfo.Msg = __sp_3 + ": " + __sp_3.SqlValue;
                _log.Debug(logInfo);
            }
#endif

            return __sps.ToArray();
        }
        #endregion
    }
}
