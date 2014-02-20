using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Blog.Model
{
    public class ArchiveChild
    {
        public String M2 { get; set; }

        private List<Article> _articles = new List<Article>();

        public List<Article> Articles
        {
            get { return _articles; }
        }
    }
}
