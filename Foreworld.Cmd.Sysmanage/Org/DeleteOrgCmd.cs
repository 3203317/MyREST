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

namespace Foreworld.Cmd.Sysmanage.Org
{
    [Implementation("deleteOrg", Description = "删除组织", Version = "1.0.0.0")]
    class DeleteOrgCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DeleteOrgCmd));

        [Parameter("ids", "组织IDS", Required = true, Regexp = "^(\\d{1,10},?)*\\d{1,10}$", RegexpInfo = "支持逗号分割字符串")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: deleteOrg [删除组织]");

            string[] __ids = @parameter.Parameters["ids"].Split(',');

            List<SqlParameter> __sps = new List<SqlParameter>();

            _log.Debug("ids: " + @parameter.Parameters["ids"]);

            string __appendSQL = string.Empty;

            for (int __i_3 = 0, __j_3 = __ids.Length; __i_3 < __j_3; __i_3++)
            {
                __appendSQL += ",@id" + __i_3;
                SqlParameter __sp_4 = new SqlParameter("@id" + __i_3, SqlDbType.Int, 10);
                __sp_4.Value = __ids[__i_3];
                __sps.Add(__sp_4);
            }

            string __sql = Resource.DeleteOrgCmd_Sql.Replace("@ids", __appendSQL);

            _log.Debug(__sps);
            _log.Debug(__sql);

            int __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, __sql, __sps.ToArray());

            string __result = "{'size':'" + __size + "'}";

            _log.Debug(__result);

            return __result;
        }
    }
}
