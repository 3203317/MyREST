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
    [Implementation("listOrgs", Description = "组织列表", Version = "1.0.0.0")]
    class ListOrgsCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListOrgsCmd));

        [Parameter("parentid", "父ID", Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: listOrgs [组织列表]");

            List<SqlParameter> __sps = new List<SqlParameter>();

            string __appendSQL = String.Empty;

            if (@parameter.Parameters.ContainsKey("parentid") && @parameter.Parameters["parentid"].Length > 0)
            {
                __appendSQL += " and a.p_id=@p_id";
                SqlParameter __sp_3 = new SqlParameter("@p_id", SqlDbType.Int, 10);
                __sp_3.Value = @parameter.Parameters["parentid"];
                __sps.Add(__sp_3);
                _log.Debug(__sp_3 + ": " + __sp_3.SqlValue);
            }

            string __sql = Resource.ListOrgsCmd_Sql.Replace("@appendSQL", __appendSQL);

            _log.Debug(__sps);
            _log.Debug(__sql);

            DataSet __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, __sql, __sps.ToArray());

            //找不到数据
            if (__ds == null) throw new Exception(Resource.ListOrgsCmd_Err);


            //JSON对象创建
            StringWriter __sw = new StringWriter();
            JsonWriter __jw = new JsonWriter(__sw);

#if DEBUG
            __jw.Formatting = Formatting.Indented;
#endif

            __jw.WriteStartObject();

            __jw.WritePropertyName("identifier");
            __jw.WriteValue("id");

            __jw.WritePropertyName("label");
            __jw.WriteValue("orgname");

            __jw.WritePropertyName("items");
            __jw.WriteStartArray();

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            DataColumnCollection __columns = __dt.Columns;

            for (int __i_3 = 0, __j_3 = __rows.Count, __k_3 = __columns.Count; __i_3 < __j_3; __i_3++)
            {
                __jw.WriteStartObject();

                DataRow __row_4 = __rows[__i_3];

                for (int __i_5 = 0; __i_5 < __k_3; __i_5++)
                {
                    __jw.WritePropertyName(__columns[__i_5].ToString());
                    __jw.WriteValue(__row_4[__i_5].ToString());
                }

                __jw.WritePropertyName("type");
                __jw.WriteValue("org");

                __jw.WriteEndObject();
            }

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
