﻿using System;
using System.Collections.Generic;
using System.Text;

using Foreworld.Cmd.Blog.Model;

namespace Foreworld.Cmd.Blog.Service
{
    public interface ArticleService : IService
    {
        List<Article> FindArticles(Pagination @pagination);

        List<Article> FindArticlesByCateId(string categoryId, Pagination @pagination);

        List<Article> FindArticlesByTagName(string tagName, Pagination @pagination);

        List<Article> GetTopMarks();

        List<Article> GetTop10ViewNums();

        Article FindById(string @id);

        Article FindNextById(string @id);

        Article FindPrevById(string @id);
    }
}
