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

namespace Foreworld.Cmd.Sysmanage.Org
{
    [Implementation("updateOrg", Description = "更新组织", Version = "1.0.0.0")]
    class UpdateOrgCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateOrgCmd));

        [Parameter("id", "组织ID", Required = true, Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        [Parameter("orgname", "组织名称", Required = true, Regexp = "^.{2,20}$", RegexpInfo = "长度为2-20位任意字符")]
        [Parameter("orgdesc", "组织描述")]
        [Parameter("orgtypeid", "组织类型", Required = true, Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        [Parameter("sort", "排序", Regexp = "^\\d{1,3}$", RegexpInfo = "支持数字", DefaultValue = "1")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: updateOrg [更新组织]");

            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@id", SqlDbType.Int);
            __sp.Value = @parameter.Parameters["id"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@orgname", SqlDbType.NVarChar, 20);
            __sp.Value = @parameter.Parameters["orgname"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@orgdesc", SqlDbType.NVarChar, 100);
            __sp.Value = @parameter.Parameters["orgdesc"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@tab_s_orgtype_id", SqlDbType.Int);
            __sp.Value = @parameter.Parameters["orgtypeid"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@sort", SqlDbType.Int, 3);
            __sp.Value = @parameter.Parameters["sort"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            _log.Debug(__sps);
            _log.Debug(Resource.UpdateOrgCmd_Sql);

            DataSet __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.UpdateOrgCmd_Sql, __sps.ToArray());


            //找不到记录
            if (__ds == null) throw new Exception(Resource.UpdateOrgCmd_Err);

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count == 0)
            {
                __ds.Clear();
                __ds.Dispose();
                throw new Exception(Resource.UpdateOrgCmd_Err);
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
            __jw.WriteValue("org");

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
