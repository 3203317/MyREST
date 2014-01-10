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

namespace Foreworld.Cmd.BookManage.BookType
{
    [Implementation("addBookType", Description = "添加图书类型", Version = "1.0.0.0")]
    class AddBookTypeCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AddBookTypeCmd));

        [Parameter("code", "编码", Required = true, Regexp = "^[A-Z1-9][A-Z0-9]{0,19}$", RegexpInfo = "编码为必填项")]
        [Parameter("parentcode", "父编码", Required = true, Regexp = "^[A-Z0-9]{1,20}$", RegexpInfo = "父编码为必填项")]
        [Parameter("booktypename", "图书类型名称", Required = true, Regexp = "^[\u4E00-\u9FA5]{1,10}$", RegexpInfo = "中文必填项")]
        [Parameter("booktypedesc", "图书类型描述")]
        [Parameter("sort", "排序", Regexp = "^\\d{1,3}$", RegexpInfo = "支持数字", DefaultValue = "1")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: addBookType [添加图书类型]");

            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@code", SqlDbType.VarChar, 20);
            __sp.Value = @parameter.Parameters["code"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@tab_p_codetype_id", SqlDbType.VarChar, 20);
            __sp.Value = "BOOKTYPE";
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@p_code", SqlDbType.VarChar, 20);
            __sp.Value = @parameter.Parameters["parentcode"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@codename", SqlDbType.NVarChar, 20);
            __sp.Value = @parameter.Parameters["booktypename"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@codedesc", SqlDbType.NVarChar, 100);
            __sp.Value = @parameter.Parameters["booktypedesc"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@sort", SqlDbType.Int, 3);
            __sp.Value = @parameter.Parameters["sort"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@addtime", SqlDbType.DateTime);
            __sp.Value = DateTime.Now;
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@opt_s_user_id", SqlDbType.Int);
            __sp.Value = @parameter.UserInfo["id"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@isenable", SqlDbType.Int);
            __sp.Value = 1;
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            __sp = new SqlParameter("@invalid", SqlDbType.Int);
            __sp.Value = 1;
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            _log.Debug(__sps);
            _log.Debug(Resource.AddBookTypeCmd_Sql);

            DataSet __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.AddBookTypeCmd_Sql, __sps.ToArray());


            //找不到新记录
            if (__ds == null) throw new Exception(Resource.AddBookTypeCmd_Err);

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count == 0)
            {
                __ds.Clear();
                __ds.Dispose();
                throw new Exception(Resource.AddBookTypeCmd_Err);
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
            __jw.WriteValue("booktype");

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
