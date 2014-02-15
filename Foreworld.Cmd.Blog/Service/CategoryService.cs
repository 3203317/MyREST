using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Blog.Service
{
    using Category = Foreworld.Cmd.Blog.Model.Category;

    public interface CategoryService : IService
    {
        List<Category> GetCategorys();

        Category FindByName(string @name);
    }
}
