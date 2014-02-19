using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Blog.Model
{
    public class Archive
    {
        public Int32? Y4 { get; set; }

        public List<ArchiveChild> ArchiveChildren { get; set; }
    }
}
