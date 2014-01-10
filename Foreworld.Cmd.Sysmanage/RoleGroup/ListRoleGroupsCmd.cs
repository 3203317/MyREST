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

namespace Foreworld.Cmd.Sysmanage.RoleGroup
{
    [Implementation("listRoleGroups", Description = "角色组列表", Version = "1.0.0.0")]
    class ListRoleGroupsCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListRoleGroupsCmd));

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            DataSet __ds = this.GetDataSet(__logInfo);
            if (__ds == null) return Util.ExceptionLog(Resource.ListRoleGroupsCmd_Err);

            string __result = this.GetJSON(__logInfo, __ds);

            return __result;
        }

        #region DataSet
        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="logInfo"></param>
        /// <returns>返回数据集</returns>
        private DataSet GetDataSet(LogInfo @logInfo)
        {
#if DEBUG
            @logInfo.Msg = Resource.ListRoleGroupsCmd_Sql;
            _log.Debug(@logInfo);
#endif

            DataSet __ds = null;

            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.ListRoleGroupsCmd_Sql);
            }
            catch (Exception @ex)
            {
                @logInfo.Code = Resource.ListRoleGroupsCmd_Err_Code;
                @logInfo.Msg = Resource.ListRoleGroupsCmd_Err;
                _log.Error(@logInfo, @ex);
            }

            return __ds;
        }
        #endregion

        #region JSON对象
        /// <summary>
        /// JSON转换字符串
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="ds">数据集</param>
        /// <returns>JSON格式</returns>
        private string GetJSON(LogInfo @logInfo, DataSet @ds)
        {
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
            __jw.WriteValue("rolegroupdesc");

            __jw.WritePropertyName("items");
            __jw.WriteStartArray();


            DataTable __dt = @ds.Tables[0];
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
                __jw.WriteValue("rolegroup");

                __jw.WriteEndObject();
            }

            __jw.WriteEndArray();
            __jw.WriteEndObject();

            __jw.Flush();

            string __result = __sw.GetStringBuilder().ToString();

            @ds.Clear();
            @ds.Dispose();
            __jw.Close();
            __sw.Close();
            __sw.Dispose();

#if DEBUG
            @logInfo.Msg = __result;
            _log.Debug(@logInfo);
#endif

            return __result;
        }
        #endregion
    }
}
