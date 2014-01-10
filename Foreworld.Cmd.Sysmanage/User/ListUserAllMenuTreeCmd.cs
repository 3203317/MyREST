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

namespace Foreworld.Cmd.Sysmanage.User
{
    [Implementation("listUserAllMenuTree", Description = "用户常用菜单树列表", Version = "1.0.0.0")]
    class ListUserAllMenuTreeCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListUserMenuTreeCmd));

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            __logInfo.Msg = "[用户常用菜单树列表]";
            _log.Debug(__logInfo);

            #region SQL参数
            SqlParameter[] __sps = { new SqlParameter("@userid", SqlDbType.Int) };

            __sps[0].Value = @parameter.UserInfo["id"];

            __logInfo.Msg = __sps[0] + ": " + __sps[0].SqlValue;
            _log.Debug(__logInfo);

            __logInfo.Msg = Resource.ListUserMenuTreeCmd_Sql;
            _log.Debug(__logInfo);
            #endregion

            #region DataSet
            DataSet __ds = null;

            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.ListUserMenuTreeCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Msg = Resource.ListUserMenuTreeCmd_Err;
                _log.Error(__logInfo, @ex);
            }


            //找不到菜单树列表
            if (__ds == null) return Util.ExceptionLog(Resource.ListUserMenuTreeCmd_Err);
            #endregion

            #region JSON对象
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
            __jw.WriteValue("modulename");

            __jw.WritePropertyName("items");
            __jw.WriteStartArray();

            DataTable __dt = __ds.Tables[0];
            DataColumnCollection __columns = __dt.Columns;
            DataRowCollection __rows = __dt.Rows;

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
                __jw.WriteValue("module");

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
            #endregion

            __logInfo.Msg = __result;
            _log.Debug(__logInfo);

            return __result;
        }
    }
}
