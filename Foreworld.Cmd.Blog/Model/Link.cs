using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Blog.Model
{
    [Table("F_LINK")]
    public class Link
    {
        public const string LINK_ORDER = "LinkOrder";

        [Column("主键", Id = true, Length = 33, Nullable = false, Unique = true)]
        public String Id { get; set; }

        [Column("链接名称")]
        public String LinkName { get; set; }

        [Column("链接地址")]
        public String LinkUrl { get; set; }

        [Column("链接图片")]
        public String LinkImage { get; set; }

        [Column("链接排序")]
        public Int32? LinkOrder { get; set; }

        [Column("是否显示")]
        public Int32? IsShow { get; set; }

        [Column("链接类型")]
        public Int32? LinkType { get; set; }
    }
}
