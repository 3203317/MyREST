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
        public List<Archive> GetArchives()
        {
            List<Archive> list = new List<Archive>();

            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Article.POST_TIME, "DESC");
            List<Article> articles = _articleDao.queryAll(null, sort, null);

            foreach (Article article_3 in articles)
            {
                if (0 == list.Count)
                {
                    /* 添加年 */
                    Archive archive_4 = new Archive();
                    archive_4.Y4 = article_3.PostTime_Year;
                    list.Add(archive_4);

                    /* 添加月 */
                    ArchiveChild archiveChild_4 = new ArchiveChild();
                    archiveChild_4.M2 = article_3.PostTime_Month;
                    archive_4.ArchiveChildren.Add(archiveChild_4);

                    archiveChild_4.Articles.Add(article_3);
                }
                else
                {
                    Archive archive_4 = list[list.Count - 1];

                    if (article_3.PostTime_Year == archive_4.Y4)
                    {
                    }
                    else
                    {
                        Archive archive_5 = new Archive();
                        archive_5.Y4 = article_3.PostTime_Year;
                        list.Add(archive_5);
                    }
                }
            }

            return list;
        }

    }
}
