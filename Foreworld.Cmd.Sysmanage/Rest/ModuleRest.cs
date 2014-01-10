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

using Foreworld.Cmd.Privilege.Service;
using Foreworld.Cmd.Privilege.Service.Impl;

namespace Foreworld.Cmd.Sysmanage.Rest
{
    using Module = Foreworld.Cmd.Privilege.Model.Module;

    public class ModuleRest : BaseRest
    {
        private ModuleService _moduleService;

        public ModuleRest()
        {
            _moduleService = new ModuleServiceImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(ModuleRest));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Resource]
        public ResultMapper IndexUI(Parameter @parameter)
        {
            List<Module> list = _moduleService.GetModuleTree();

            IContext vltCtx = new VelocityContext();
            vltCtx.Put("title", "模块管理");
            vltCtx.Put("moduleTree", JavaScriptConvert.SerializeObject(list));

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
