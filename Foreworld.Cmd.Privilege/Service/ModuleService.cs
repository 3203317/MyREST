using System;
using System.Collections.Generic;
using System.Text;

using Foreworld.Cmd.Privilege.Model;

namespace Foreworld.Cmd.Privilege.Service
{
    public interface ModuleService : IService
    {
        /// <summary>
        /// 获取全部的模块列表
        /// </summary>
        /// <returns></returns>
        List<Module> GetModuleTree();
    }
}
