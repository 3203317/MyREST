#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;

using log4net;

namespace Foreworld.Cmd.Blog.Dao.Impl
{
    using Category = Foreworld.Cmd.Blog.Model.Category;

    public class CategoryDaoImpl : OleBaseDao<Category, Category>, CategoryDao
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CategoryDaoImpl));
    }
}
