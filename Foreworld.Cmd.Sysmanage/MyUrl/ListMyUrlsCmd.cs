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
    [Implementation("listMyUrls", Description = "我的网址列表", Version = "1.0.0.0")]
    class ListMyUrlsCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListMyUrlsCmd));

        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: listMyUrls [我的网址列表]");

            SqlParameter[] __sps = new SqlParameter[1];

            __sps[0] = new SqlParameter("@opt_s_user_id", SqlDbType.Int);
            __sps[0].Value = @parameter.UserInfo["id"];
            _log.Debug(__sps[0] + ": " + __sps[0].SqlValue);

            _log.Debug(__sps);
            _log.Debug(Resource.ListMyUrlsCmd_Sql);

            DataSet __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.ListMyUrlsCmd_Sql, __sps);

            //找不到数据
            if (__ds == null) throw new Exception(Resource.ListMyUrlsCmd_Err);


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
            __jw.WriteValue("url");

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
                __jw.WriteValue("myurl");

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
