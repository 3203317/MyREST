using System;
using System.Collections.Generic;
using System.Text;

using log4net;

using Foreworld.Cmd.Blog.Model;
using Foreworld.Cmd.Blog.Dao;
using Foreworld.Cmd.Blog.Dao.Impl;

namespace Foreworld.Cmd.Blog.Service.Impl
{
    public class LinkServiceImpl : BaseService, LinkService
    {
        private LinkDao _linkDao;

        public LinkServiceImpl()
        {
            _linkDao = new LinkDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(LinkServiceImpl));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Link> GetUsefulLinks()
        {
            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Link.LINK_ORDER, "ASC");

            Link search = new Link();
            search.LinkType = 1;

            List<Link> list = _linkDao.queryAll(null, sort, search);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Link> GetFriendlyLinks()
        {
            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Link.LINK_ORDER, "ASC");

            Link search = new Link();
            search.LinkType = 2;

            List<Link> list = _linkDao.queryAll(null, sort, search);
            return list;
        }
    }
}
