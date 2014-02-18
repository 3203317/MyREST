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
        public List<Article> FindArticles(Pagination @pagination)
        {
            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Article.POST_TIME, "DESC");

            List<Article> list = _articleDao.queryAll(@pagination, sort, null);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<Article> FindArticlesByCateId(string categoryId, Pagination @pagination)
        {
            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Article.POST_TIME, "DESC");

            Article search = new Article();
            search.CategoryId = categoryId;

            List<Article> list = _articleDao.queryAll(@pagination, sort, search);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<Article> FindArticlesByTagName(string tagName, Pagination @pagination)
        {
            string sql = "SELECT * FROM F_ARTICLE  WHERE upper(ArticleTag) like ?ArticleTag ORDER BY PostTime DESC";

            Article article = new Article();
            article.ArticleTag = "%," + tagName.ToUpper() + ",%";

            List<Article> list = _articleDao.queryAll(sql, article, @pagination);
            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Article FindById(string @id)
        {
            Article article = new Article();
            article.Id = @id;
            return _articleDao.query(article);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Article> GetTopMarks()
        {
            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Article.POST_TIME, "DESC");

            Article search = new Article();
            search.TopMark = 1;

            List<Article> list = _articleDao.queryAll(null, sort, search);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Article> GetTop10ViewNums()
        {
            Pagination pagination = new Pagination();
            pagination.Current = 1;
            pagination.PageSize = 10;

            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Article.VIEW_NUMS, "DESC");

            List<Article> list = _articleDao.queryAll(pagination, sort, null);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Article FindNextById(string @id)
        {
            string sql = "SELECT * FROM F_ARTICLE WHERE PostTime<(SELECT PostTime FROM F_ARTICLE WHERE ID = ?Id) ORDER BY PostTime DESC";

            Article article = new Article();
            article.Id = @id;

            Pagination pagination = new Pagination();
            pagination.Current = 0;
            pagination.PageSize = 1;

            List<Article> list = _articleDao.queryAll(sql, article, pagination);
            if (null == list || 0 == list.Count) return null;

            article = list[0];
            return article;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Article FindPrevById(string @id)
        {
            string sql = "SELECT * FROM F_ARTICLE WHERE PostTime>(SELECT PostTime FROM F_ARTICLE WHERE ID = ?Id) ORDER BY PostTime";

            Article article = new Article();
            article.Id = @id;

            Pagination pagination = new Pagination();
            pagination.Current = 0;
            pagination.PageSize = 1;

            List<Article> list = _articleDao.queryAll(sql, article, pagination);
            if (null == list || 0 == list.Count) return null;

            article = list[0];
            return article;
        }
    }
}
