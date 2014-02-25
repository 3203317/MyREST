using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Reflection;
using System.IO;

using log4net;
using Newtonsoft.Json;

using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity.Context;

using Foreworld.Log;
using Foreworld.Cmd.Blog.Model;
using Foreworld.Cmd.Blog.Service;
using Foreworld.Cmd.Blog.Service.Impl;

namespace Foreworld.Cmd.Blog.Rest
{
    public class ArticleRest : BaseRest
    {
        private TagService _tagService;
        private ArchiveService _archiveService;
        private ArticleService _articleService;
        private CommentService _commentService;

        private static volatile VelocityEngine _vltEngine;
        private static object _syncObj = new Object();

        public ArticleRest()
        {
            _tagService = new TagServiceImpl();
            _archiveService = new ArchiveServiceImpl();
            _articleService = new ArticleServiceImpl();
            _commentService = new CommentServiceImpl();
            Init();
        }

        private void Init()
        {
            if (null == _vltEngine)
            {
                lock (_syncObj)
                {
                    if (null == _vltEngine)
                    {
                        _vltEngine = new VelocityEngine();
                        _vltEngine.SetProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
                        _vltEngine.SetProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
                        _vltEngine.Init();
                    }
                }
            }
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(ArticleRest));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="htmlObj"></param>
        /// <param name="fileName"></param>
        private void CreateHtml(Parameter @parameter, HtmlObject htmlObj, string fileName)
        {
            HttpContext httpContext = @parameter.HttpContext;

            StringWriter vltWriter = new StringWriter();
            _vltEngine.Evaluate(htmlObj.Context, vltWriter, null, htmlObj.Template);

            using (StreamWriter sw = new StreamWriter(httpContext.Server.MapPath(fileName), false, Encoding.UTF8, 200))
            {
                sw.Write(vltWriter);
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// 标签
        /// </summary>
        /// <param name="parameter"></param>
        private void CreateTagList(Parameter @parameter)
        {
            IContext vltCtx = new VelocityContext();
            vltCtx.Put("virtualPath", "../../");
            vltCtx.Put("tags", _tagService.GetTags());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate("pagelet.TagList");
            htmlObj.Context = vltCtx;

            CreateHtml(@parameter, htmlObj, "~/App_Data/pagelet/tagList.html");
        }

        /// <summary>
        /// 热门文章
        /// </summary>
        /// <param name="parameter"></param>
        private void CreateTop10ViewNums(Parameter @parameter)
        {
            IContext vltCtx = new VelocityContext();
            vltCtx.Put("virtualPath", "/");
            vltCtx.Put("top10ViewNums", _articleService.GetTop10ViewNums());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate("pagelet.Top10ViewNums");
            htmlObj.Context = vltCtx;

            CreateHtml(@parameter, htmlObj, "~/App_Data/pagelet/top10ViewNums.html");
        }

        /// <summary>
        /// 档案馆
        /// </summary>
        /// <param name="parameter"></param>
        private void CreateArchiveList(Parameter @parameter)
        {
            IContext vltCtx = new VelocityContext();
            vltCtx.Put("virtualPath", "../");
            vltCtx.Put("archives", _archiveService.GetArchives());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate("pagelet.ArchiveList");
            htmlObj.Context = vltCtx;

            CreateHtml(@parameter, htmlObj, "~/App_Data/pagelet/archiveList.html");
        }

        /// <summary>
        /// 最新10条评论
        /// </summary>
        /// <param name="parameter"></param>
        private void CreateTop10Comments(Parameter @parameter)
        {
            IContext vltCtx = new VelocityContext();
            vltCtx.Put("virtualPath", "/");
            vltCtx.Put("top10Comments", _commentService.GetTop10Comments());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate("pagelet.Top10Comments");
            htmlObj.Context = vltCtx;

            CreateHtml(@parameter, htmlObj, "~/App_Data/pagelet/top10Comments.html");
        }

        delegate void CreateHtmlDelegate(Parameter @parameter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource(Public = true)]
        public ResultMapper Add(Parameter @parameter)
        {
            CreateHtmlDelegate createHtml = new CreateHtmlDelegate(CreateTagList);
            createHtml += CreateArchiveList;
            createHtml += CreateTop10ViewNums;
            createHtml += CreateTop10Comments;
            createHtml(@parameter);

            ResultMapper mapper = new ResultMapper();
            mapper.Success = true;
            return mapper;
        }
    }
}
