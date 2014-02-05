using System;
using System.Collections.Generic;
using System.Text;

using Foreworld.Cmd.Blog.Model;

namespace Foreworld.Cmd.Blog.Service
{
    public interface CommentService : IService
    {
        List<Comment> GetTop10Comments();
    }
}
