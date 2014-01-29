using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Reflection;

using log4net;
using Newtonsoft.Json;

using NVelocity;
using NVelocity.Context;

using Foreworld.Log;
using Foreworld.Cmd.Blog.Service;
using Foreworld.Cmd.Blog.Service.Impl;

namespace Foreworld.Cmd.Blog.Rest
{
    using Category = Foreworld.Cmd.Blog.Model.Category;

    public class IndexRest : BaseRest
    {
        private CategoryService _categoryService;
        private ArticleService _articleService;

        public IndexRest()
        {
            _categoryService = new CategoryServiceImpl();
            _articleService = new ArticleServiceImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(IndexRest));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource]
        public ResultMapper IndexUI(Parameter @parameter)
        {
            IContext vltCtx = new VelocityContext();
            vltCtx.Put("title", "FOREWORLD 洪荒");
            vltCtx.Put("categorys", _categoryService.GetCategorys());
            vltCtx.Put("articles", _articleService.GetArticles());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate();
            htmlObj.Context = vltCtx;

            ResultMapper mapper = new ResultMapper();
            mapper.Data = htmlObj;
            mapper.Success = true;
            return mapper;
        }
    }
}
