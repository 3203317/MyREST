#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using log4net;

using Foreworld.Cmd.Privilege.Model;
using Foreworld.Cmd.Privilege.Dao;
using Foreworld.Cmd.Privilege.Dao.Impl;

namespace Foreworld.Cmd.Privilege.Service.Impl
{
    public class ModuleServiceImpl : BaseService, ModuleService
    {
        private ModuleDao _moduleDao;

        public ModuleServiceImpl()
        {
            _moduleDao = new ModuleDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(ModuleServiceImpl));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Module> GetModuleTree()
        {
            Module __search = new Module();
            __search.IsEnable = 1;
            __search.IsInvalid = 1;

            Dictionary<string, string> __sort = new Dictionary<string, string>();
            __sort.Add(Module.PMODULEID, string.Empty);
            __sort.Add(Module.SORT, string.Empty);
            __sort.Add(Module.MODULEID, "ASC");

            List<Module> __list = _moduleDao.queryAll(null, __sort, __search);
            return __list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Module> GetModulesByParentId(string @id)
        {
            Module __search = new Module();
            __search.PModuleId = @id;

            Dictionary<string, string> __sort = new Dictionary<string, string>();
            __sort.Add(Module.SORT, "ASC");

            List<Module> __list = _moduleDao.queryAll(null, __sort, __search);
            return __list;
        }
    }
}
