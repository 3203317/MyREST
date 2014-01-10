#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;
using System.Reflection;

using log4net;

using Foreworld.Utils;
using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class FormParamsInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FormParamsInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            ////获取Cmd命令
            //HttpRequest __request = @context.Request;
            //ICmd __cmd = CmdManager.INSTANCE.GetCmd(__request.QueryString["command"].Trim());

            ////获取方法引用
            //Type __type = __cmd.GetType();
            //MethodInfo __methodInfo = __type.GetMethod("Execute");

            ////创建命令参数组
            //Dictionary<string, string> __parameters = new Dictionary<string, string>();
            //@context.Items.Add("parameters", __parameters);

            //foreach (ParameterAttribute __paramAttr_3 in __methodInfo.GetCustomAttributes(typeof(ParameterAttribute), false))
            //{
            //    //获取表单参数名称
            //    string __paramName_4 = __paramAttr_3.Name;

            //    //获取表单参数对象（*正式使用需要改成form方式*）
            //    //#if DEBUG
            //    object __reqObj_4 = __request[__paramName_4];
            //    //#else
            //    //                object __paramObj_4 = __request.Form[__paramName_4];
            //    //#endif

            //    //表单参数验证
            //    if (__reqObj_4 == null)
            //    {
            //        if (__paramAttr_3.Required)
            //        {
            //            Util.ExceptionLog(@context, Status.FAILURE, string.Format(Resource.err_formParamRequire, __paramName_4));
            //            break;
            //        }
            //        else
            //        {
            //            //非必填项给定默认值
            //            __parameters.Add(__paramName_4, __paramAttr_3.DefaultValue);
            //        }
            //    }
            //    else
            //    {
            //        //获取表单参数值
            //        string __reqObjVal_5 = __reqObj_4.ToString().Trim();

            //        if (__reqObjVal_5.Length == 0)
            //        {
            //            if (__paramAttr_3.Required)
            //            {
            //                Util.ExceptionLog(@context, Status.FAILURE, string.Format(Resource.err_formParamRequire, __paramName_4));
            //                break;
            //            }
            //            else
            //            {
            //                //非必填项给定默认值
            //                __parameters.Add(__paramName_4, __paramAttr_3.DefaultValue);
            //            }
            //        }
            //        else
            //        {
            //            //对表单参数值进行正则表达式验证
            //            if (__paramAttr_3.Regexp.Length > 0 && !Regex.IsMatch(__reqObjVal_5, __paramAttr_3.Regexp))
            //            {
            //                //Util.ExceptionLog(@context, Resource.err_paramValueRegex_Code, string.Format(Resource.err_paramValueRegex, __paramAttr_3.Description), __paramAttr_3.RegexpInfo);
            //                Util.ExceptionLog(@context, Status.FAILURE, string.Format(Resource.err_paramValueRegex, __paramAttr_3.Description));
            //                break;
            //            }

            //            //反射为命令参数赋值
            //            __parameters.Add(__paramName_4, __reqObjVal_5);
            //        }
            //    }
            //}

            if (@context.Items["result"] == null) successor.RequestInterceptor(@context);
        }
    }
}
