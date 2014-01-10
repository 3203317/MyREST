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

namespace Foreworld.Cmd.Sysmanage.CodeType
{
    [Implementation("listCodeTypes", Description = "代码类型列表", Version = "1.0.0.0")]
    class ListCodeTypesCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListCodeTypesCmd));

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            #region SQL参数
#if DEBUG
            __logInfo.Msg = Resource.ListCodeTypesCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region DataSet
            DataSet __ds = null;

            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.ListCodeTypesCmd_Sql);
            }
            catch (Exception @ex)
            {
                __logInfo.Code = Resource.ListCodeTypesCmd_Err_Code;
                __logInfo.Msg = Resource.ListCodeTypesCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            //找不到数据
            if (__ds == null) return Util.ExceptionLog(Resource.ListCodeTypesCmd_Err);
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
            __jw.WriteValue("codetypedesc");

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
                __jw.WriteValue("codetype");

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

#if DEBUG
            __logInfo.Msg = __result;
            _log.Debug(__logInfo);
#endif

            return __result;
        }
    }
}
