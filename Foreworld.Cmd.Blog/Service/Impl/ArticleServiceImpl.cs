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
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public List<Article> GetArticles(uint @pageSize, uint @currentPage)
        {
            string querySql = "SELECT * FROM (SELECT TOP " + @pageSize + " * FROM (SELECT TOP " + (@pageSize * @currentPage) + " * FROM F_ARTICLE ORDER BY " + Article.POST_TIME + " DESC) ORDER BY " + Article.POST_TIME + " ASC) ORDER BY " + Article.POST_TIME + " DESC";

            List<Article> __list = _articleDao.queryAll(querySql);
            return __list;
        }
    }
}
