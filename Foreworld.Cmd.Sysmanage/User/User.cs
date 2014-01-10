using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Foreworld.Cmd.Sysmanage.User
{
    [Table("_B98B9F18C3084195BDFDE3ACA7A4AD0E")]
    public class User
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("f1", "主键", Id = true, Length = 33, Nullable = false, Unique = true)]
        public String UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Column("f2", "用户名", Length = 32, SqlDbType = SqlDbType.NVarChar)]
        public String UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Column("f3", "密码", Length = 32)]
        public String UserPass { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Column("f4", "性别", Length = 1)]
        public Int32? Sex { get; set; }

        public String Sex_Text
        {
            get
            {
                switch (Sex)
                {
                    case 1:
                        return "男";
                    case 2:
                        return "女";
                    case 3:
                        return "未知";
                    default:
                        return "未知";
                }
            }
        }

        public String IsEnable_Text
        {
            get { return 1 == IsEnable ? "启用" : "禁用"; }
        }

        public String IsInvalid_Text
        {
            get { return 1 == IsInvalid ? "有效" : "无效"; }
        }

        [Column("f20", "添加时间", SqlDbType = SqlDbType.DateTime)]
        public String AddTime { get; set; }

        [Column("f21", "添加用户")]
        public String AddUserId { get; set; }
        public String AddUserId_Text { get; set; }

        [Column("f22", "编辑时间")]
        public String EditTime { get; set; }

        [Column("f23", "编辑用户")]
        public String EditUserId { get; set; }
        public String EditUserId_Text { get; set; }

        [Column("f24", "启用禁用", Length = 1)]
        public Int32? IsEnable { get; set; }

        [Column("f25", "有效无效", Length = 1)]
        public Int32? IsInvalid { get; set; }
    }
}
