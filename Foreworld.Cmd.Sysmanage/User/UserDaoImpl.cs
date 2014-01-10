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
    class UserDaoImpl : BaseDao<User, User>, UserDao
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UserDaoImpl));
    }
}
