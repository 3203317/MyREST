using System;
using System.Collections.Generic;
using System.Text;

using log4net;

using Foreworld.Cmd.Blog.Model;
using Foreworld.Cmd.Blog.Dao;
using Foreworld.Cmd.Blog.Dao.Impl;

namespace Foreworld.Cmd.Blog.Service.Impl
{
    public class ArchiveServiceImpl : BaseService, ArchiveService
    {
        private ArticleDao _articleDao;

        public ArchiveServiceImpl()
        {
            _articleDao = new ArticleDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(ArchiveServiceImpl));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Archive> Archives()
        {
            List<Archive> list = new List<Archive>();


            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Article.POST_TIME, "DESC");
            List<Article> articles = _articleDao.queryAll(null, sort, null);


            foreach (Article ss in articles)
            {
                Archive a = new Archive();
                a.Y4 = ss.PostTime_Year;

                list.Add(a);
            }

            return list;
        }

    }
}
