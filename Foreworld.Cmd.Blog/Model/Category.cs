using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Foreworld.Cmd.Blog.Model
{
    [Table("F_CATEGORY")]
    public class Category
    {
        public const string CATEGORY_ORDER = "CategoryOrder";

        [Column("主键", Id = true, Length = 33, Nullable = false, Unique = true)]
        public String Id { get; set; }

        [Column("分类名称")]
        public String CategoryName { get; set; }

        [Column("分类排序")]
        public Int32? CategoryOrder { get; set; }
    }
}
