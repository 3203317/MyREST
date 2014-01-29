using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Blog.Dao
{
    using Category = Foreworld.Cmd.Blog.Model.Category;

    public interface CategoryDao : IDao<Category, Category>
    {
    }
}
