using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd
{
    public abstract class ValidatorInterceptor
    {
        public ValidatorInterceptor Successor { get; set; }

        public virtual ResultMapper RequestInterceptor(Parameter @parameter)
        {
            ResultMapper mapper = new ResultMapper();
            mapper.Success = true;
            return mapper;
        }
    }
}