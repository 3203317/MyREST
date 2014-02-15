﻿using System;
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
            Dictionary<string, string> sort = new Dictionary<string, string>();
            sort.Add(Category.CATEGORY_ORDER, "ASC");

            List<Category> list = _categoryDao.queryAll(null, sort, null);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Category FindByName(string @name)
        {
            Category search = new Category();
            search.CategoryName = @name;
            Category category = _categoryDao.query(search);
            return category;
        }
    }
}
