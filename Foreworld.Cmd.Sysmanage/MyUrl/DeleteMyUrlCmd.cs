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
    [Implementation("deleteMyUrl", Description = "删除我的网址", Version = "1.0.0.0")]
    class DeleteMyUrlCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DeleteMyUrlCmd));

        [Parameter("ids", "网址IDS", Required = true, Regexp = "^(\\d{1,10},?)*\\d{1,10}$", RegexpInfo = "支持逗号分割字符串")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: deleteMyUrl [删除我的网址]");
            Dictionary<string, string> __parameters = @parameter.Parameters;

            string[] __ids = __parameters["ids"].Split(',');

            List<SqlParameter> __sps = new List<SqlParameter>();

            _log.Debug("ids: " + __parameters["ids"]);

            SqlParameter __sp = null;

            __sp = new SqlParameter("@opt_s_user_id", SqlDbType.Int);
            __sp.Value = @parameter.UserInfo["id"];
            __sps.Add(__sp);
            _log.Debug(__sp + ": " + __sp.SqlValue);

            string __appendSQL = string.Empty;

            for (int __i_3 = 0, __j_3 = __ids.Length; __i_3 < __j_3; __i_3++)
            {
                __appendSQL += ",@id" + __i_3;
                __sp = new SqlParameter("@id" + __i_3, SqlDbType.Int, 10);
                __sp.Value = __ids[__i_3];
                __sps.Add(__sp);
            }

            string __sql = Resource.DeleteMyUrlCmd_Sql.Replace("@ids", __appendSQL);

            _log.Debug(__sps);
            _log.Debug(__sql);

            int __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, __sql, __sps.ToArray());

            string __result = "{'size':'" + __size + "'}";

            _log.Debug(__result);

            return __result;
        }
    }
}
