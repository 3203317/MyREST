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
    [Implementation("updateCodeType", Description = "更新代码类型", Version = "1.0.0.0")]
    class UpdateCodeTypeCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateCodeTypeCmd));

        [Parameter("id", "代码类型名称", Required = true, Regexp = "^[A-Z_]{2,20}$", RegexpInfo = "代码类型名称为必填项")]
        [Parameter("codetypedesc", "代码类型描述")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            #region SQL参数
            Dictionary<string, string> __parameters = @parameter.Parameters;

            SqlParameter[] __sps = new SqlParameter[4];

            __sps[0] = new SqlParameter("@id", SqlDbType.VarChar);
            __sps[0].Value = __parameters["id"];

            __sps[1] = new SqlParameter("@codetypedesc", SqlDbType.NVarChar, 100);
            __sps[1].Value = __parameters["codetypedesc"];

            __sps[2] = new SqlParameter("@editime", SqlDbType.DateTime);
            __sps[2].Value = DateTime.Now;

            __sps[3] = new SqlParameter("@ed_s_user_id", SqlDbType.Int);
            __sps[3].Value = @parameter.UserInfo["id"];

#if DEBUG
            foreach (SqlParameter __sp_3 in __sps)
            {
                __logInfo.Msg = __sp_3 + ": " + __sp_3.SqlValue;
                _log.Debug(__logInfo);
            }

            __logInfo.Msg = Resource.UpdateCodeTypeCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region DataSet
            DataSet __ds = null;

            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.UpdateCodeTypeCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Code = Resource.UpdateCodeTypeCmd_Err_Code;
                __logInfo.Msg = Resource.UpdateCodeTypeCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            //找不到记录
            if (__ds == null) return Util.ExceptionLog(Resource.UpdateCodeTypeCmd_Err);

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count == 0)
            {
                __ds.Clear();
                __ds.Dispose();
                return Util.ExceptionLog(Resource.UpdateCodeTypeCmd_Err);
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
            __jw.WriteValue("codetype");

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

            __logInfo.Msg = string.Format(Resource.UpdateCodeTypeCmd, __sps[0].SqlValue);
            _log.Info(__logInfo);

            return __result;
        }
    }
}
