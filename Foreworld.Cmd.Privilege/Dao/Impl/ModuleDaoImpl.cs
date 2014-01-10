#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;

using log4net;

using Foreworld.Cmd.Privilege.Model;

namespace Foreworld.Cmd.Privilege.Dao.Impl
{
    public class ModuleDaoImpl : BaseDao<Module, Module>, ModuleDao
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ModuleDaoImpl));
    }
}
