using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Foreworld.Cmd.Blog.Model
{
    [Table("F_ARTICLE")]
    public class Article
    {
        public const string POST_TIME = "PostTime";

        [Column("主键", Id = true, Length = 33, Nullable = false, Unique = true)]
        public String Id { get; set; }

        [Column("文章标题")]
        public String ArticleTitle { get; set; }

        [Column("文章介绍")]
        public String ArticleIntro { get; set; }

        [Column("文章内容")]
        public String ArticleContent { get; set; }

        [Column("文章作者")]
        public String ArticleAuthor { get; set; }

        [Column("发布时间", OleDbType = OleDbType.DBTimeStamp)]
        public String PostTime { get; set; }

        [Column("阅读次数")]
        public Int32? ViewNums { get; set; }

    }
}
