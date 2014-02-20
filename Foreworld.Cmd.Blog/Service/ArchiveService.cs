using System;
using System.Collections.Generic;
using System.Text;

using Foreworld.Cmd.Blog.Model;

namespace Foreworld.Cmd.Blog.Service
{
    public interface ArchiveService : IService
    {
        List<Archive> Archives();
    }
}
