using System;
using System.Collections.Generic;
using System.Text;

using log4net;

using Foreworld.Cmd.Blog.Model;
using Foreworld.Cmd.Blog.Dao;
using Foreworld.Cmd.Blog.Dao.Impl;

namespace Foreworld.Cmd.Blog.Service.Impl
{
    public class TagServiceImpl : BaseService, TagService
    {
        private TagDao _tagDao;
        private ArticleDao _articleDao;

        public TagServiceImpl()
        {
            _tagDao = new TagDaoImpl();
            _articleDao = new ArticleDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(TagServiceImpl));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Tag> GetTags()
        {
            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Article.POST_TIME, "DESC");

            List<Article> articleList = _articleDao.queryAll(null, sort, null);

            sort = new Dictionary<string, string>();
            sort.Add(Tag.TAG_NAME, "ASC");

            List<Tag> tagList = _tagDao.queryAll(null, sort, null);

            foreach (Tag tag_3 in tagList)
            {
                foreach (Article article_3 in articleList)
                {
                    if (null != article_3.ArticleTag && 0 < article_3.ArticleTag.Length && -1 < article_3.ArticleTag.IndexOf("," + tag_3.TagName + ","))
                    {
                        tag_3.Articles.Add(article_3);
                    }
                }
            }

            return tagList;
        }
    }
}
