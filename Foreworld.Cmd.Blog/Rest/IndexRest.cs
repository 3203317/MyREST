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
using Foreworld.Cmd.Blog.Model;
using Foreworld.Cmd.Blog.Service;
using Foreworld.Cmd.Blog.Service.Impl;

namespace Foreworld.Cmd.Blog.Rest
{
    using Category = Foreworld.Cmd.Blog.Model.Category;

    public class IndexRest : BaseRest
    {
        private CategoryService _categoryService;
        private ArticleService _articleService;
        private CommentService _commentService;
        private LinkService _linkService;

        public IndexRest()
        {
            _categoryService = new CategoryServiceImpl();
            _articleService = new ArticleServiceImpl();
            _commentService = new CommentServiceImpl();
            _linkService = new LinkServiceImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(IndexRest));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource(Public = true)]
        public ResultMapper IndexUI(Parameter @parameter)
        {
            Pagination pagination = new Pagination();
            pagination.PageSize = 10;
            pagination.Current = 1;

            IContext vltCtx = new VelocityContext();
            vltCtx.Put("moduleName", "index");
            vltCtx.Put("virtualPath", string.Empty);

            vltCtx.Put("title", "FOREWORLD 洪荒");
            vltCtx.Put("description", "个人博客");
            vltCtx.Put("keywords", "Bootstrap3");
            vltCtx.Put("topMessage", "欢迎您。今天是" + DateTime.Now.ToString("yyyy年MM月dd日") + "。");
            vltCtx.Put("categorys", _categoryService.GetCategorys());
            vltCtx.Put("articles", _articleService.FindArticles(pagination));
            vltCtx.Put("top10Comments", _commentService.GetTop10Comments());
            vltCtx.Put("usefulLinks", _linkService.GetUsefulLinks());
            vltCtx.Put("topMarks", _articleService.GetTopMarks());
            vltCtx.Put("top10ViewNums", _articleService.GetTop10ViewNums());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate();
            htmlObj.Context = vltCtx;

            ResultMapper mapper = new ResultMapper();
            mapper.Data = htmlObj;
            mapper.Success = true;
            return mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource(Public = true)]
        public ResultMapper LoadMoreUI(Parameter @parameter)
        {
            string dataStr = GetDataStr(@parameter);
            Pagination pagination = JavaScriptConvert.DeserializeObject<Pagination>(dataStr);
            pagination.PageSize = 10;

            IContext vltCtx = new VelocityContext();
            vltCtx.Put("articles", _articleService.FindArticles(pagination));

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate();
            htmlObj.Context = vltCtx;

            ResultMapper mapper = new ResultMapper();
            mapper.Data = htmlObj;
            mapper.Success = true;
            return mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource(Public = true)]
        public ResultMapper ArticleUI(Parameter @parameter)
        {
            Pagination pagination = new Pagination();
            pagination.PageSize = 10;
            pagination.Current = 1;

            HttpRequest request = @parameter.HttpContext.Request;
            string id = request.QueryString["id"];

            Article article = _articleService.FindById(id);

            IContext vltCtx = new VelocityContext();
            vltCtx.Put("moduleName", "archives");
            vltCtx.Put("virtualPath", "../");

            vltCtx.Put("title", "FOREWORLD 洪荒");
            vltCtx.Put("atitle", article.ArticleTitle);
            vltCtx.Put("description", "个人博客");
            vltCtx.Put("keywords", "Bootstrap3");
            vltCtx.Put("topMessage", "欢迎您。今天是" + DateTime.Now.ToString("yyyy年MM月dd日") + "。");
            vltCtx.Put("categorys", _categoryService.GetCategorys());
            vltCtx.Put("top10Comments", _commentService.GetTop10Comments());
            vltCtx.Put("usefulLinks", _linkService.GetUsefulLinks());
            vltCtx.Put("top10ViewNums", _articleService.GetTop10ViewNums());
            vltCtx.Put("article", article);
            vltCtx.Put("nextArticle", _articleService.FindNextById(id));
            vltCtx.Put("prevArticle", _articleService.FindPrevById(id));

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate();
            htmlObj.Context = vltCtx;

            ResultMapper mapper = new ResultMapper();
            mapper.Data = htmlObj;
            mapper.Success = true;
            return mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource(Public = true)]
        public ResultMapper ArchiveUI(Parameter @parameter)
        {
            IContext vltCtx = new VelocityContext();
            vltCtx.Put("moduleName", "archives");
            vltCtx.Put("virtualPath", string.Empty);

            vltCtx.Put("title", "FOREWORLD 洪荒");
            vltCtx.Put("atitle", "档案馆");
            vltCtx.Put("description", "个人博客");
            vltCtx.Put("keywords", "Bootstrap3");
            vltCtx.Put("topMessage", "欢迎您。今天是" + DateTime.Now.ToString("yyyy年MM月dd日") + "。");
            vltCtx.Put("categorys", _categoryService.GetCategorys());
            vltCtx.Put("top10Comments", _commentService.GetTop10Comments());
            vltCtx.Put("usefulLinks", _linkService.GetUsefulLinks());
            vltCtx.Put("top10ViewNums", _articleService.GetTop10ViewNums());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate();
            htmlObj.Context = vltCtx;

            ResultMapper mapper = new ResultMapper();
            mapper.Data = htmlObj;
            mapper.Success = true;
            return mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource(Public = true)]
        public ResultMapper CategoryUI(Parameter @parameter)
        {
            HttpRequest request = @parameter.HttpContext.Request;
            string categoryName = request.QueryString["categoryName"];

            IContext vltCtx = new VelocityContext();
            vltCtx.Put("moduleName", "category");
            vltCtx.Put("virtualPath", "../../");

            vltCtx.Put("title", "FOREWORLD 洪荒");
            vltCtx.Put("atitle", categoryName);
            vltCtx.Put("categoryName", categoryName);
            vltCtx.Put("description", "个人博客");
            vltCtx.Put("keywords", "Bootstrap3");
            vltCtx.Put("topMessage", "欢迎您。今天是" + DateTime.Now.ToString("yyyy年MM月dd日") + "。");
            vltCtx.Put("categorys", _categoryService.GetCategorys());
            vltCtx.Put("top10Comments", _commentService.GetTop10Comments());
            vltCtx.Put("usefulLinks", _linkService.GetUsefulLinks());
            vltCtx.Put("topMarks", _articleService.GetTopMarks());
            vltCtx.Put("top10ViewNums", _articleService.GetTop10ViewNums());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate();
            htmlObj.Context = vltCtx;

            ResultMapper mapper = new ResultMapper();
            mapper.Data = htmlObj;
            mapper.Success = true;
            return mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource(Public = true)]
        public ResultMapper TagUI(Parameter @parameter)
        {
            HttpRequest request = @parameter.HttpContext.Request;
            string tagName = request.QueryString["tagName"];

            IContext vltCtx = new VelocityContext();
            vltCtx.Put("moduleName", "tag");
            vltCtx.Put("virtualPath", "../../");

            vltCtx.Put("title", "FOREWORLD 洪荒");
            vltCtx.Put("atitle", tagName);
            vltCtx.Put("tagName", tagName);
            vltCtx.Put("description", "个人博客");
            vltCtx.Put("keywords", "Bootstrap3");
            vltCtx.Put("topMessage", "欢迎您。今天是" + DateTime.Now.ToString("yyyy年MM月dd日") + "。");
            vltCtx.Put("categorys", _categoryService.GetCategorys());
            vltCtx.Put("top10Comments", _commentService.GetTop10Comments());
            vltCtx.Put("usefulLinks", _linkService.GetUsefulLinks());
            vltCtx.Put("top10ViewNums", _articleService.GetTop10ViewNums());

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
