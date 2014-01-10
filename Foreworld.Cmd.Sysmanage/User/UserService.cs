using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Sysmanage.User
{
    public interface UserService : IService
    {
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="loginForm">登陆表单</param>
        /// <returns>用户</returns>
        ResultMapper Login(LoginForm @loginForm);

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        ResultMapper Logout();
    }
}
