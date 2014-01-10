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

namespace Foreworld.Cmd.Sysmanage.MyUrl
{
    [Implementation("updateMyUrl", Description = "更新我的网址", Version = "1.0.0.0")]
    class UpdateMyUrlCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateMyUrlCmd));

        [Parameter("id", "网址ID", Required = true, Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        [Parameter("url", "网址", Required = true, Regexp = "^.{15,100}$", RegexpInfo = "必填项")]
        [Parameter("memo", "说明")]
        [Parameter("ispublic", "公共", Regexp = "^\\d{1}$", RegexpInfo = "支持数字", DefaultValue = "2")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: updateMyUrl [更新我的网址]");
            Dictionary<string, string> __parameters = @parameter.Parameters;

            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@opt_s_user_id", SqlDbType.Int);
            __sp.Value = @parameter.UserInfo["id"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@id", SqlDbType.Int, 10);
            __sp.Value = __parameters["id"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@url", SqlDbType.VarChar, 100);
            __sp.Value = __parameters["url"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@memo", SqlDbType.NVarChar, 100);
            __sp.Value = __parameters["memo"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@tab_p_code_ispublic_id", SqlDbType.Int, 1);
            __sp.Value = __parameters["ispublic"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            _log.Debug(__sps);
            _log.Debug(Resource.UpdateMyUrlCmd_Sql);

            DataSet __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.UpdateMyUrlCmd_Sql, __sps.ToArray());


            //找不到记录
            if (__ds == null) throw new Exception(Resource.UpdateMyUrlCmd_Err);

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count == 0)
            {
                __ds.Clear();
                __ds.Dispose();
                throw new Exception(Resource.UpdateMyUrlCmd_Err);
            }

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

            _log.Debug(__result);

            return __result;
        }
    }
}
