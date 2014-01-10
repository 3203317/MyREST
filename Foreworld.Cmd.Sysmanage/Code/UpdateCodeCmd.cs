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
    [Implementation("updateCode", Description = "更新代码", Version = "1.0.0.0")]
    class UpdateCodeCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateCodeCmd));

        [Parameter("code", "编码", Required = true, Regexp = "^[A-Z1-9][A-Z0-9_]{0,19}$", RegexpInfo = "编码为必填项")]
        [Parameter("codetypeid", "代码类型名称", Required = true, Regexp = "^[A-Z_]{2,20}$", RegexpInfo = "代码类型名称为必填项")]
        [Parameter("codename", "代码名称", Required = true, Regexp = "^[\u4E00-\u9FA5]{1,10}$", RegexpInfo = "中文必填项")]
        [Parameter("codedesc", "代码描述")]
        [Parameter("sort", "排序", Regexp = "^\\d{1,3}$", RegexpInfo = "支持数字", DefaultValue = "1")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            #region SQL参数
            Dictionary<string, string> __parameters = @parameter.Parameters;

            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@code", SqlDbType.VarChar, 20);
            __sp.Value = __parameters["code"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@tab_p_codetype_id", SqlDbType.VarChar, 20);
            __sp.Value = __parameters["codetypeid"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@codename", SqlDbType.NVarChar, 20);
            __sp.Value = __parameters["codename"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@codedesc", SqlDbType.NVarChar, 100);
            __sp.Value = __parameters["codedesc"];
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

            __logInfo.Msg = Resource.UpdateCodeCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region DataSet
            DataSet __ds = null;

            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.UpdateCodeCmd_Sql, __sps.ToArray());
            }
            catch (Exception @ex)
            {
                __logInfo.Code = Resource.UpdateCodeCmd_Err_Code;
                __logInfo.Msg = Resource.UpdateCodeCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            //找不到记录
            if (__ds == null) return Util.ExceptionLog(Resource.UpdateCodeCmd_Err);

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count == 0)
            {
                __ds.Clear();
                __ds.Dispose();
                return Util.ExceptionLog(Resource.UpdateCodeCmd_Err);
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

            DataRow __row = __dt.Rows[0];
            DataColumnCollection __columns = __dt.Columns;

            for (int __i_3 = 0, __j_3 = __columns.Count; __i_3 < __j_3; __i_3++)
            {
                __jw.WritePropertyName(__columns[__i_3].ToString());
                __jw.WriteValue(__row[__i_3].ToString());
            }

            __jw.WritePropertyName("type");
            __jw.WriteValue("code");

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
            __logInfo.Msg = string.Format(Resource.UpdateCodeCmd, __parameters["code"] + "," + __parameters["codetypeid"]);
            _log.Info(__logInfo);

            return __result;
        }
    }
}
