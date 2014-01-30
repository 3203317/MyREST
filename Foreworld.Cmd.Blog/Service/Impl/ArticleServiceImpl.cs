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
        public List<Article> GetArticles(int pageSize, int currentPage)
        {
            Article __search = new Article();

            Dictionary<string, string> __sort = new Dictionary<string, string>();
            __sort.Add(Article.POST_TIME, "DESC");

            List<Article> __list = _articleDao.queryAll(null, __sort, __search);
            return __list;
        }
    }
}
