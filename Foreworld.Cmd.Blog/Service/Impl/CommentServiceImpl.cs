using System;
using System.Collections.Generic;
using System.Text;

using log4net;

using Foreworld.Cmd.Blog.Model;
using Foreworld.Cmd.Blog.Dao;
using Foreworld.Cmd.Blog.Dao.Impl;

namespace Foreworld.Cmd.Blog.Service.Impl
{
    public class CommentServiceImpl : BaseService, CommentService
    {
        private CommentDao _commentDao;

        public CommentServiceImpl()
        {
            _commentDao = new CommentDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(CommentServiceImpl));

        public List<Comment> GetTop10Comments()
        {
            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Comment.Post_Time, "DESC");

            List<Comment> list = _commentDao.queryAll(10, sort, null);
            return list;
        }
    }
}
