using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using MySql.Data.MySqlClient;

namespace Foreworld.Cmd.Blog.Model
{
    [Table("F_ARTICLE")]
    public class Article
    {
        public const string POST_TIME = "PostTime";
        public const string VIEW_NUMS = "ViewNums";

        [Column("主键", Id = true, Length = 32, Nullable = false, Unique = true, MySqlDbType = MySqlDbType.VarChar)]
        public String Id { get; set; }

        [Column("类型Id", Length = 32, Nullable = false, MySqlDbType = MySqlDbType.VarChar)]
        public String CategoryId { get; set; }

        [Column("文章标题")]
        public String ArticleTitle { get; set; }

        [Column("文章介绍")]
        public String ArticleIntro { get; set; }

        [Column("文章作者")]
        public String ArticleAuthor { get; set; }

        [Column("文章内容")]
        public String ArticleContent { get; set; }

        [Column("发布时间", MySqlDbType = MySqlDbType.Datetime)]
        public String PostTime { get; set; }

        public string PostTime_Year
        {
            get { return Convert.ToDateTime(PostTime).Year.ToString(); }
        }

        public string PostTime_Month
        {
            get { return Convert.ToDateTime(PostTime).Month.ToString().PadLeft(2, '0'); }
        }

        public string PostTime_Day
        {
            get { return Convert.ToDateTime(PostTime).Day.ToString().PadLeft(2, '0'); }
        }

        public string PostTime_Date
        {
            get { return Convert.ToDateTime(PostTime).ToShortDateString(); }
        }

        public string PostTime_Time
        {
            get { return Convert.ToDateTime(PostTime).ToLongTimeString(); }
        }

        [Column("阅读次数")]
        public Int32? ViewNums { get; set; }

        public String ViewNumsToFormat
        {
            get { return double.Parse(ViewNums.ToString()).ToString("#,#"); }
        }

        [Column("文章标签", MySqlDbType = MySqlDbType.VarChar, Length = 32)]
        public String ArticleTag { get; set; }

        public List<Tag> Tags
        {
            get
            {
                if (null == ArticleTag || 0 == ArticleTag.Trim().Length) return null;

                string[] tags = ArticleTag.Trim().Split(',');

                List<Tag> list = new List<Tag>();

                for (int i = 0, j = tags.Length; i < j; i++)
                {
                    string tagName_3 = tags[i];
                    if (!string.IsNullOrEmpty(tagName_3))
                    {
                        Tag tag_3 = new Tag();
                        tag_3.TagName = tagName_3.Trim();

                        list.Add(tag_3);
                    }
                }

                return list;
            }
        }

        [Column("书签")]
        public Int32? Bookmark { get; set; }

        [Column("置顶", MySqlDbType = MySqlDbType.Int32, Length = 2)]
        public Int32? TopMark { get; set; }

        [Column("文章图片")]
        public String ArticleImage { get; set; }

    }
}
