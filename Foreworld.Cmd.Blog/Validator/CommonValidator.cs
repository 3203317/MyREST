using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;

using log4net;

using Foreworld.Log;

namespace Foreworld.Cmd.Blog.Validator
{
    public class CommonValidator : IValidator
    {
        public CommonValidator()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override ResultMapper Validate(Parameter @parameter)
        {
            ResultMapper mapper = new ResultMapper();
            mapper.Success = true;
            return mapper;
        }
    }
}
