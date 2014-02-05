#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;

using log4net;
using Foreworld.Cmd.Blog.Model;

namespace Foreworld.Cmd.Blog.Dao.Impl
{
    public class CommentDaoImpl : OleBaseDao<Comment, Comment>, CommentDao
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CommentDaoImpl));
    }
}
