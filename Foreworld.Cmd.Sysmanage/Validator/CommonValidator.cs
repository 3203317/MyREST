using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;

using log4net;

using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.Validator
{
    public class CommonValidator : IValidator
    {
        private ValidatorInterceptor _ticketInterceptor;
        private ValidatorInterceptor _resourceInterceptor;

        public CommonValidator()
        {
            _ticketInterceptor = new TicketInterceptor();
            _resourceInterceptor = new ResourceInterceptor();

            _ticketInterceptor.Successor = _resourceInterceptor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override ResultMapper Validate(Parameter @parameter)
        {
            return _ticketInterceptor.RequestInterceptor(@parameter);
        }
    }
}
