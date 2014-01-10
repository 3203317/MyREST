using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd
{
    public abstract class IValidator
    {
        public virtual ResultMapper Validate(Parameter @parameter)
        {
            ResultMapper mapper = new ResultMapper();
            mapper.Success = true;
            return mapper;
        }

        public virtual void Destroy() { }
    }
}
