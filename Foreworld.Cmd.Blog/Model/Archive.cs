using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Blog.Model
{
    public class Archive
    {
        public String Y4 { get; set; }

        private List<ArchiveChild> _archiveChildren = new List<ArchiveChild>();

        public List<ArchiveChild> ArchiveChildren
        {
            get { return _archiveChildren; }
        }
    }
}
