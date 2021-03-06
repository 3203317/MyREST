﻿using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;

namespace Foreworld.Cmd.Blog.Model
{
    [Table("F_TAG")]
    public class Tag
    {
        public const string TAG_NAME = "TagName";

        [Column("主键", Id = true, Length = 32, Nullable = false, Unique = true, MySqlDbType = MySqlDbType.VarChar)]
        public String Id { get; set; }

        [Column("标签名称")]
        public String TagName { get; set; }

        [Column("标签数量")]
        public Int32? TagCount { get; set; }

        private List<Article> _articles = new List<Article>();

        public List<Article> Articles
        {
            get { return _articles; }
        }
    }
}
