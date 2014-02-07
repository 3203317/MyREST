﻿using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;

namespace Foreworld.Cmd.Blog.Model
{
    [Table("F_COMMENT")]
    public class Comment
    {
        public const string Post_Time = "PostTime";

        [Column("主键", Id = true, Length = 33, Nullable = false, Unique = true)]
        public String Id { get; set; }

        [Column("文章主键")]
        public String ArticleId { get; set; }

        [Column("内容")]
        public String Content { get; set; }

        [Column("作者")]
        public String Author { get; set; }

        [Column("发布时间", MySqlDbType = MySqlDbType.Datetime)]
        public String PostTime { get; set; }

        [Column("发布IP")]
        public String PostIP { get; set; }
    }
}
