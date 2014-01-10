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

namespace Foreworld.Cmd.BookManage.Book
{
    [Implementation("listBooks", Description = "图书列表", Version = "1.0.0.0")]
    class ListBooksCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListBooksCmd));

        public override object Execute(Parameter @parameter)
        {
            string __result = "{'size':'123'}";
            return __result;
        }
    }
}
