using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Foreworld.Cmd.Privilege.Model
{
    [Table("S_ROLE")]
    public class Role
    {
        [Column("主键")]
        public String RoleId { get; set; }

        [Column("角色名称")]
        public String RoleName { get; set; }

        [Column("角色描述")]
        public String RoleDesc { get; set; }

        [Column("开始时间", SqlDbType = SqlDbType.DateTime)]
        public String StartTime { get; set; }

        [Column("结束时间", SqlDbType = SqlDbType.DateTime)]
        public String EndTime { get; set; }

        [Column("添加时间", SqlDbType = SqlDbType.DateTime)]
        public String AddTime { get; set; }

        [Column("添加用户")]
        public String AddUserId { get; set; }
        public String AddUserId_Text { get; set; }

        [Column("编辑时间", SqlDbType = SqlDbType.DateTime)]
        public String EditTime { get; set; }

        [Column("编辑用户")]
        public String EditUserId { get; set; }
        public String EditUserId_Text { get; set; }

        [Column("启用禁用", Length = 1)]
        public Int32? IsEnable { get; set; }
        public String IsEnable_Text { get { return 1 == IsEnable ? "启用" : "禁用"; } }

        [Column("有效无效", Length = 1)]
        public Int32? IsInvalid { get; set; }
        public String IsInvalid_Text { get { return 1 == IsInvalid ? "有效" : "无效"; } }
    }
}
