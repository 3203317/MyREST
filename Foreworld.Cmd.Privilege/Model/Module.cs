using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace Foreworld.Cmd.Privilege.Model
{
    [Table("S_MODULE")]
    public class Module
    {
        /// <summary>
        /// PModuleId
        /// </summary>
        public const string PMODULEID = "PModuleId";

        /// <summary>
        /// Sort
        /// </summary>
        public const string SORT = "Sort";

        /// <summary>
        /// ModuleId
        /// </summary>
        public const string MODULEID = "ModuleId";


        [Column("主键", Id = true, Length = 33, Nullable = false, Unique = true)]
        public String ModuleId { get; set; }

        [Column("父主键")]
        public String PModuleId { get; set; }

        [Column("模块名称")]
        public String ModuleName { get; set; }

        [Column("模块地址")]
        public String ModuleUrl { get; set; }

        [Column("排序")]
        public Int32? Sort { get; set; }

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
