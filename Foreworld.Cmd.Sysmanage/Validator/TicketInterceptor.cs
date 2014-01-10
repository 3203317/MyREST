using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;

using NVelocity;
using NVelocity.Context;

using Foreworld.Cmd;

namespace Foreworld.Cmd.Sysmanage.Validator
{
    /// <summary>
    /// 票据权限验证
    /// </summary>
    public class TicketInterceptor : ValidatorInterceptor
    {
        private static readonly string _htmlTemplate = @"<!DOCTYPE HTML>
<HTML>
    <HEAD>
        <TITLE>Error Page</TITLE>
        <meta http-equiv='content-type' content='text/html;charset=utf-8'>
    </HEAD>
    <BODY>
        <h1>Unknown Error Page.</h1>
        <h2>Please Visit <a href='http://www.foreworld.net'>http://www.foreworld.net</a>.</h2>
    </BODY>
</HTML>";

        public override ResultMapper RequestInterceptor(Parameter @parameter)
        {
            HttpContext httpContext = @parameter.HttpContext;
            HttpCookie cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (null == cookie && !httpContext.User.Identity.IsAuthenticated)
            {
                ResultMapper mapper_3 = new ResultMapper();

                switch (@parameter.RequestType)
                {
                    case RequestType.HTML:
                        {
                            IContext vltCtx_4 = new VelocityContext();

                            HtmlObject htmlObj_4 = new HtmlObject();
                            htmlObj_4.Context = vltCtx_4;
                            htmlObj_4.Template = _htmlTemplate;

                            mapper_3.Data = htmlObj_4;
                            break;
                        }
                    case RequestType.JSON:
                        {
                            mapper_3.Msg = "会话超时";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                return mapper_3;
            }
            else
            {
                return Successor.RequestInterceptor(@parameter);
            }
        }
    }
}
