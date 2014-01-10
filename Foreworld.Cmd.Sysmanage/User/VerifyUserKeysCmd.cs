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
    [Implementation("verifyUserKeys", Description = "用户APIKey验证", Version = "1.0.0.0")]
    class VerifyUserKeysCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(VerifyUserKeysCmd));

        public override AccessLevel Access
        {
            get { return AccessLevel.PRIVATE; }
        }

        [Parameter("apikey", "apikey", Required = true, Regexp = "^[a-zA-Z0-9]{6,16}$", RegexpInfo = "必填项")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

#if DEBUG
            __logInfo.Msg = "[用户APIKey验证]";
            _log.Debug(__logInfo);
#endif

            #region SQL参数
            SqlParameter[] __sps = { new SqlParameter("@apikey", SqlDbType.VarChar, 50) };
            __sps[0].Value = @parameter.Parameters["apikey"];

#if DEBUG
            __logInfo.Msg = __sps[0] + ": " + __sps[0].SqlValue;
            _log.Debug(__logInfo);

            __logInfo.Msg = Resource.VerifyUserKeysCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            #region DataSet
            DataSet __ds = null;

            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.VerifyUserKeysCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Msg = Resource.VerifyUserKeysCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            //找不到该用户的apikey
            if (__ds == null)
            {
                string __result_3 = Util.ExceptionLog(Resource.VerifyUserKeysCmd_Err);
#if DEBUG
                __logInfo.Msg = __result_3;
                _log.Error(__logInfo);
#endif
                return __result_3;
            }

            DataTable __dt = __ds.Tables[0];
            DataRowCollection __rows = __dt.Rows;
            if (__rows.Count != 1)
            {
                __ds.Clear();
                __ds.Dispose();
                string __result_3 = Util.ExceptionLog(Resource.VerifyUserKeysCmd_Err);
#if DEBUG
                __logInfo.Msg = __result_3;
                _log.Error(__logInfo);
#endif
                return __result_3;
            }
            #endregion

            #region JSON对象
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

            DataColumnCollection __columns = __dt.Columns;
            DataRow __row = __rows[0];

            for (int __i_3 = 0, __k_3 = __columns.Count; __i_3 < __k_3; __i_3++)
            {
                __jw.WritePropertyName(__columns[__i_3].ToString());
                __jw.WriteValue(__row[__i_3].ToString());
            }
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
            #endregion

#if DEBUG
            __logInfo.Msg = __result;
            _log.Debug(__logInfo);
#endif
            return __result;
        }
    }
}
