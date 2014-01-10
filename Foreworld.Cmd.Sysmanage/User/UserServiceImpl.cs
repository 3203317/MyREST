#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Security;

using log4net;
using Newtonsoft.Json;

using Foreworld.Cmd;
using Foreworld.Db;
using Foreworld.Utils;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.User
{
    public class UserServiceImpl : BaseService, UserService
    {
        private UserDao _userDao;

        public UserServiceImpl()
        {
            _userDao = new UserDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(UserServiceImpl));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginForm"></param>
        /// <returns></returns>
        public ResultMapper Login(LoginForm @loginForm)
        {
            User __user = new User();
            __user.UserName = @loginForm.UserName;
            __user.IsEnable = 1;
            __user.IsInvalid = 1;

            __user = _userDao.query(__user);

            ResultMapper __mapper = new ResultMapper();

            if (null == __user)
            {
                __mapper.Msg = "查询用户失败";
            }
            else
            {
                if (!Utils.MD5.Encrypt(@loginForm.UserPass).ToLower().Equals(__user.UserPass.ToLower()))
                {
                    __mapper.Msg = "用户名或密码输入错误";
                }
                else
                {
                    __user.UserPass = null;
                    __mapper.Data = __user;
                    __mapper.Status = ResultMapper.StatusType.SUCCESS;
                }
            }
            return __mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResultMapper Logout()
        {
            ResultMapper __mapper = new ResultMapper();
            __mapper.Status = ResultMapper.StatusType.SUCCESS;
            return __mapper;
        }
    }
}
