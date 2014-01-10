using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Foreworld.Cmd.Privilege.Model
{
    [Table("S_USER")]
    public class User
    {
        [Column("主键", Id = true, Length = 33, Nullable = false, Unique = true)]
        public String UserId { get; set; }

        [Column("用户名", Length = 32, SqlDbType = SqlDbType.NVarChar)]
        public String UserName { get; set; }

        [Column("密码", Length = 32, DefaultValue = "e10adc3949ba59abbe56e057f20f883e")]
        public String UserPass { get; set; }

        [Column("性别", Length = 1, DefaultValue = "2")]
        public Int32? Sex { get; set; }
        public String Sex_Text
        {
            get
            {
                switch (Sex)
                {
                    case 1: return "男";
                    case 2: return "女";
                    default: return "未知";
                }
            }
        }

        [Column("添加时间", SqlDbType = SqlDbType.DateTime)]
        public String AddTime { get; set; }

        [Column("添加用户", DefaultValue = "1")]
        public String AddUserId { get; set; }
        public String AddUserId_Text { get; set; }

        [Column("编辑时间", SqlDbType = SqlDbType.DateTime)]
        public String EditTime { get; set; }

        [Column("编辑用户")]
        public String EditUserId { get; set; }
        public String EditUserId_Text { get; set; }

        [Column("启用禁用", Length = 1, DefaultValue = "1")]
        public Int32? IsEnable { get; set; }
        public String IsEnable_Text { get { return 1 == IsEnable ? "启用" : "禁用"; } }

        [Column("有效无效", Length = 1, DefaultValue = "1")]
        public Int32? IsInvalid { get; set; }
        public String IsInvalid_Text { get { return 1 == IsInvalid ? "有效" : "无效"; } }
    }
}
