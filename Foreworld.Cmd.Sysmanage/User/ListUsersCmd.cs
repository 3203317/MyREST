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

namespace Foreworld.Cmd.Sysmanage.User
{
    [Implementation("listUsers", Description = "用户列表", Version = "1.0.0.0")]
    class ListUsersCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListUsersCmd));

        [Parameter("username", "用户名", Regexp = "^.{1,19}$", RegexpInfo = "限制长度")]
        [Parameter("sex", "性别", Regexp = "^\\d{1}$", RegexpInfo = "支持数字")]
        [Parameter("realname", "真实姓名", Regexp = "^.{1,19}$", RegexpInfo = "限制长度")]
        [Parameter("regtime1", "注册日期起", Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        [Parameter("regtime2", "注册日期止", Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        [Parameter("currentpage", "当前页码", Required = true, Regexp = "^[1-9]\\d{0,9}$", RegexpInfo = "支持数字", DefaultValue = "1")]
        [Parameter("pagesize", "每页显示记录数", Required = true, Regexp = "^(20|50|100|200)$", RegexpInfo = "支持数字", DefaultValue = "20")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: listUsers (用户列表)");

            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@currentpage", SqlDbType.Int, 10);
            __sp.Value = @parameter.Parameters["currentpage"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@pagesize", SqlDbType.Int, 4);
            __sp.Value = @parameter.Parameters["pagesize"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            string __appendSQL = String.Empty;

            if (@parameter.Parameters.ContainsKey("username") && @parameter.Parameters["username"].Length > 0)
            {
                __appendSQL += " and t.username like @username";
                __sp = new SqlParameter("@username", SqlDbType.NVarChar, 20);
                __sp.Value = "%" + @parameter.Parameters["username"] + "%";
                __sps.Add(__sp);
                _log.Debug(__sp + ": " + __sp.SqlValue);
            }

            if (@parameter.Parameters.ContainsKey("sex") && @parameter.Parameters["sex"].Length > 0)
            {
                __appendSQL += " and t.sex = @sex";
                __sp = new SqlParameter("@sex", SqlDbType.Int, 1);
                __sp.Value = @parameter.Parameters["sex"];
                __sps.Add(__sp);
                _log.Debug(__sp + ": " + __sp.SqlValue);
            }

            if (@parameter.Parameters.ContainsKey("realname") && @parameter.Parameters["realname"].Length > 0)
            {
                __appendSQL += " and t.firstname+t.lastname like @realname";
                __sp = new SqlParameter("@realname", SqlDbType.NVarChar, 20);
                __sp.Value = "%" + @parameter.Parameters["realname"] + "%";
                __sps.Add(__sp);
                _log.Debug(__sp + ": " + __sp.SqlValue);
            }

            if (@parameter.Parameters.ContainsKey("regtime1") && @parameter.Parameters["regtime1"].Length > 0)
            {
                if (@parameter.Parameters.ContainsKey("regtime2") && @parameter.Parameters["regtime2"].Length > 0)
                {
                    int __regtime1_3 = Convert.ToInt32(@parameter.Parameters["regtime1"]);
                    int __regtime2_3 = Convert.ToInt32(@parameter.Parameters["regtime2"]);

                    if (__regtime1_3 <= __regtime2_3)
                    {
                        __appendSQL += " and DATEDIFF(s,'1970-01-01 00:00:00.000',t.addtime)-8*60*60 between between @regtime1 and @regtime2";

                        __sp = new SqlParameter("@regtime1", SqlDbType.Int, 10);
                        __sp.Value = __regtime1_3;
                        __sps.Add(__sp);
                        _log.Debug(__sp + ": " + __sp.SqlValue);

                        __sp = new SqlParameter("@regtime2", SqlDbType.Int, 10);
                        __sp.Value = __regtime2_3;
                        __sps.Add(__sp);
                        _log.Debug(__sp + ": " + __sp.SqlValue);
                    }
                }
            }

            string __sql = Resource.ListUsersCmd_Sql.Replace("@appendSQL", __appendSQL);

            _log.Debug(__sps);
            _log.Debug(__sql);

            DataSet __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, __sql, __sps.ToArray());

            //找不到数据
            if (__ds == null) throw new Exception(Resource.ListUsersCmd_Err);


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
            __jw.WriteValue("username");

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
                __jw.WriteValue("user");

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
