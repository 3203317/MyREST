using System;
using System.Collections.Generic;
using System.Text;

using log4net;

using Foreworld.Cmd.Blog.Dao;
using Foreworld.Cmd.Blog.Dao.Impl;

namespace Foreworld.Cmd.Blog.Service.Impl
{
    using Category = Foreworld.Cmd.Blog.Model.Category;

    public class CategoryServiceImpl : BaseService, CategoryService
    {
        private CategoryDao _categoryDao;

        public CategoryServiceImpl()
        {
            _categoryDao = new CategoryDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(CategoryServiceImpl));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCategorys()
        {
            Category __search = new Category();

            Dictionary<string, string> __sort = new Dictionary<string, string>();
            __sort.Add(Category.CATEGORY_ORDER, "ASC");

            List<Category> __list = _categoryDao.queryAll(null, __sort, __search);
            return __list;
        }
    }
}
