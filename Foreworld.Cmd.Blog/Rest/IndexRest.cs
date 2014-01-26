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

namespace Foreworld.Cmd.Blog.Rest
{
    public class IndexRest : BaseRest
    {
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
