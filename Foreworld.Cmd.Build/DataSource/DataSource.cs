using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Build.DataSource
{
    [Serializable]
    public class DataSource
    {
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string DsName { get; set; }
        public int DsType { get; set; }
        public string DsDesc { get; set; }
        public string SqlStr { get; set; }
        public string TestParams { get; set; }
        public int SafeType { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 添加用户
        /// </summary>
        public int AddUserId { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 编辑用户
        /// </summary>
        public int EditUserId { get; set; }

        /// <summary>
        /// 启用禁用
        /// </summary>
        public int IsEnable { get; set; }

        /// <summary>
        /// 有效无效
        /// </summary>
        public int IsValid { get; set; }


        public string DsType_Text
        {
            get
            {
                return DsType == 1 ? "SQL" : "PROC";
            }
        }

        public string SafeType_Text
        {
            get
            {
                return SafeType == 1 ? "受保护" : "公开";
            }
        }

        public string IsEnable_Text
        {
            get
            {
                return IsEnable == 1 ? "启用" : "禁用";
            }
        }

        public string IsValid_Text
        {
            get
            {
                return IsValid == 1 ? "有效" : "无效";
            }
        }
    }
}
