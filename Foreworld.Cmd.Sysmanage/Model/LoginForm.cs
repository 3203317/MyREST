using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Sysmanage.Model
{
    class LoginForm
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPass { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
    }
}
