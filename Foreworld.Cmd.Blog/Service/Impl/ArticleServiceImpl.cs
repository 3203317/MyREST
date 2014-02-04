using System;
using System.Collections.Generic;
using System.Text;

using log4net;

using Foreworld.Cmd.Blog.Model;
using Foreworld.Cmd.Blog.Dao;
using Foreworld.Cmd.Blog.Dao.Impl;

namespace Foreworld.Cmd.Blog.Service.Impl
{
    public class ArticleServiceImpl : BaseService, ArticleService
    {
        private ArticleDao _articleDao;

        public ArticleServiceImpl()
        {
            _articleDao = new ArticleDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(ArticleServiceImpl));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<Article> GetArticles(Pagination @pagination)
        {
            string querySql = "SELECT * FROM (SELECT TOP " + @pagination.PageSize + " * FROM (SELECT TOP " + (@pagination.PageSize * @pagination.Current) + " * FROM F_ARTICLE ORDER BY " + Article.POST_TIME + " DESC) ORDER BY " + Article.POST_TIME + " ASC) ORDER BY " + Article.POST_TIME + " DESC";

            List<Article> list = _articleDao.queryAll(querySql);
            return list;
        }
    }
}
