using System;
using System.Collections.Generic;
using System.Text;

using Foreworld.Cmd.Privilege.Model;

namespace Foreworld.Cmd.Privilege.Service
{
    public interface UserService : IService
    {
        /// <summary>
        /// 通过用户名查找用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        User FindUserByName(string @name);

        /// <summary>
        /// 通过用户Id获得菜单树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Module> GetMenuTreeById(string @id);
    }
}
