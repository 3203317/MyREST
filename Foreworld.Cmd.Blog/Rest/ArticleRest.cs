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

        public ArticleRest()
        {
            _tagService = new TagServiceImpl();
            _archiveService = new ArchiveServiceImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(ArticleRest));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        private void CreateHtml1(Parameter @parameter)
        {
            HttpContext httpContext = @parameter.HttpContext;

            VelocityEngine _vltEngine = new VelocityEngine();
            _vltEngine.SetProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            _vltEngine.SetProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            _vltEngine.Init();

            IContext vltCtx = new VelocityContext();
            vltCtx.Put("virtualPath", "../../");
            vltCtx.Put("tags", _tagService.GetTags());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate("pagelet.TagList");
            htmlObj.Context = vltCtx;

            StringWriter vltWriter = new StringWriter();
            _vltEngine.Evaluate(htmlObj.Context, vltWriter, null, htmlObj.Template);

            using (StreamWriter sw = new StreamWriter(httpContext.Server.MapPath("~/App_Data/pagelet/tagList.html"), false, Encoding.UTF8, 200))
            {
                sw.Write(vltWriter);
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        private void CreateHtml2(Parameter @parameter)
        {
            HttpContext httpContext = @parameter.HttpContext;

            VelocityEngine _vltEngine = new VelocityEngine();
            _vltEngine.SetProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            _vltEngine.SetProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            _vltEngine.Init();

            IContext vltCtx = new VelocityContext();
            vltCtx.Put("virtualPath", "../");
            vltCtx.Put("archives", _archiveService.GetArchives());

            HtmlObject htmlObj = new HtmlObject();
            htmlObj.Template = GetVltTemplate("pagelet.ArchiveList");
            htmlObj.Context = vltCtx;

            StringWriter vltWriter = new StringWriter();
            _vltEngine.Evaluate(htmlObj.Context, vltWriter, null, htmlObj.Template);

            using (StreamWriter sw = new StreamWriter(httpContext.Server.MapPath("~/App_Data/pagelet/archiveList.html"), false, Encoding.UTF8, 200))
            {
                sw.Write(vltWriter);
                sw.Flush();
                sw.Close();
            }
        }

        delegate void CreateHtml(Parameter @parameter);

        [Resource(Public = true)]
        public ResultMapper Add(Parameter @parameter)
        {
            CreateHtml createHtml = new CreateHtml(CreateHtml1);
            createHtml += new CreateHtml(CreateHtml2);
            createHtml(@parameter);

            ResultMapper mapper = new ResultMapper();
            mapper.Success = true;
            return mapper;
        }
    }
}
