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
    [Implementation("deleteBookType", Description = "删除图书类型", Version = "1.0.0.0")]
    class DeleteBookTypeCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DeleteBookTypeCmd));

        [Parameter("ids", "代码ID", Required = true, Regexp = "^[A-Z1-9][A-Z0-9]{0,19},[A-Z]{2,20}$", RegexpInfo = "代码ID必填项")]
        public override object Execute(Parameter @parameter)
        {
            _log.Debug("Cmd: deleteBookType [删除图书类型]");

            string[] __ids = @parameter.Parameters["ids"].Split(',');

            SqlParameter[] __sps = new SqlParameter[2];

            __sps[0] = new SqlParameter("@code", SqlDbType.VarChar, 20);
            __sps[0].Value = __ids[0].Trim();
            _log.Debug(__sps[0] + ": " + __sps[0].SqlValue);

            __sps[1] = new SqlParameter("@tab_p_codetype_id", SqlDbType.VarChar, 20);
            __sps[1].Value = __ids[1].Trim();
            _log.Debug(__sps[1] + ": " + __sps[1].SqlValue);

            _log.Debug(__sps);
            _log.Debug(Resource.DeleteBookTypeCmd_Sql);

            int __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Resource.DeleteBookTypeCmd_Sql, __sps);

            string __result = "{'size':'" + __size + "'}";

            _log.Debug(__result);

            return __result;
        }
    }
}
