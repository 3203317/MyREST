#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

using log4net;
using Newtonsoft.Json;

using Foreworld.Cmd;
using Foreworld.Db;
using Foreworld.Utils;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.User
{
    [Implementation("logout", Description = "用户退出", Version = "1.0.0.0")]
    public class LogoutCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LogoutCmd));

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            __logInfo.Msg = "[用户退出]";
            _log.Debug(__logInfo);

            #region SQL参数
            SqlParameter[] __sps = { new SqlParameter("@logoutime", SqlDbType.DateTime), new SqlParameter("@id", SqlDbType.Int) };

            __sps[0].Value = DateTime.Now;

            __sps[1].Value = @parameter.UserInfo["id"];


            foreach (SqlParameter __sp_3 in __sps)
            {
                __logInfo.Msg = __sp_3 + ": " + __sp_3.SqlValue;
                _log.Debug(__logInfo);
            }

            __logInfo.Msg = Resource.LogoutCmd_Sql;
            _log.Debug(__logInfo);
            #endregion

            #region SQL执行
            int __size = 0;
            try
            {
                __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Resource.LogoutCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Msg = Resource.LogoutCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            if (__size == 0) return Util.ExceptionLog(Resource.LogoutCmd_Err);
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

            __jw.WritePropertyName("id");
            __jw.WriteValue(__sps[1].Value.ToString());

            __jw.WritePropertyName("logoutime");
            __jw.WriteValue(__sps[0].Value.ToString());

            __jw.WriteEndObject();

            __jw.WriteEndArray();
            __jw.WriteEndObject();

            __jw.Flush();

            string __result = __sw.GetStringBuilder().ToString();

            __jw.Close();
            __sw.Close();
            __sw.Dispose();
            #endregion

            __logInfo.Msg = __result;
            _log.Debug(__logInfo);

            __logInfo.Msg = Resource.LogoutCmd;
            _log.Info(__logInfo);

            return __result;
        }
    }
}
