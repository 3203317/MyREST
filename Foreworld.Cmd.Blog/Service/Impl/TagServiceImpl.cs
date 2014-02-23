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

        public TagServiceImpl()
        {
            _tagDao = new TagDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(TagServiceImpl));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Tag> GetTags()
        {
            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Tag.TAG_NAME, "ASC");

            List<Tag> list = _tagDao.queryAll(null, sort, null);
            return list;
        }
    }
}
