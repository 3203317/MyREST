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
    [Implementation("updateUser", Description = "更新用户", Version = "1.0.0.0")]
    class UpdateUserCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UpdateUserCmd));

        [Parameter("id", "用户名", Required = true, Regexp = "^[\u4E00-\u9FA5a-zA-Z0-9_]{2,10}$", RegexpInfo = "2-10个字符，支持中英文大小写、数字、下划线")]
        [Parameter("sex", "性别", Required = true, Regexp = "^\\d{1}$", RegexpInfo = "支持数字", DefaultValue = "1")]
        [Parameter("alias", "别名", Regexp = "^[\u4E00-\u9FA5a-zA-Z0-9_]{2,10}$", RegexpInfo = "2-10个字符，支持中英文大小写、数字、下划线")]
        [Parameter("firstname", "姓", Regexp = "^[\u4E00-\u9FA5]{1,4},[\u4E00-\u9FA5]{1,4}$", RegexpInfo = "姓名规范")]
        [Parameter("lastname", "名", Regexp = "^[\u4E00-\u9FA5]{1,4},[\u4E00-\u9FA5]{1,4}$", RegexpInfo = "姓名规范")]
        [Parameter("qq", "QQ", Regexp = "^\\d{5,15}$", RegexpInfo = "支持数字")]
        [Parameter("email", "电子邮箱")]
        [Parameter("idcard", "身份证")]
        [Parameter("mobile", "手机")]
        [Parameter("fax", "传真")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: updateUser (更新用户)");

            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@id", SqlDbType.NVarChar, 10);
            __sp.Value = @parameter.Parameters["id"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@sex", SqlDbType.Int, 1);
            __sp.Value = @parameter.Parameters["sex"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@alias", SqlDbType.NVarChar, 20);
            __sp.Value = @parameter.Parameters["alias"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@firstname", SqlDbType.NVarChar, 20);
            __sp.Value = @parameter.Parameters["firstname"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@lastname", SqlDbType.NVarChar, 20);
            __sp.Value = @parameter.Parameters["lastname"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@qq", SqlDbType.VarChar, 20);
            __sp.Value = @parameter.Parameters["qq"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@email", SqlDbType.VarChar, 50);
            __sp.Value = @parameter.Parameters["email"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@idcard", SqlDbType.VarChar, 50);
            __sp.Value = @parameter.Parameters["idcard"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@mobile", SqlDbType.VarChar, 50);
            __sp.Value = @parameter.Parameters["mobile"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@fax", SqlDbType.VarChar, 50);
            __sp.Value = @parameter.Parameters["fax"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            _log.Debug(__sps);
            _log.Debug(Resource.UpdateUserCmd_Sql);

            DataSet __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.UpdateUserCmd_Sql, __sps.ToArray());


            //找不到记录
            if (__ds == null) throw new Exception(Resource.UpdateUserCmd_Err);

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count == 0)
            {
                __ds.Clear();
                __ds.Dispose();
                throw new Exception(Resource.UpdateModuleCmd_Err);
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
            __jw.WriteValue("user");

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

