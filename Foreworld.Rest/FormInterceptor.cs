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
    class FormInterceptor : IHttpModule
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FormInterceptor));

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication __application = (HttpApplication)sender;
            HttpRequest __request = __application.Request;
            if (!__request.CurrentExecutionFilePath.EndsWith("/Api.ashx", StringComparison.OrdinalIgnoreCase)) return;

            HttpContext __context = __application.Context;


            //获取Get方式传递的参数command
            string __command = __request.QueryString["command"] == null ? string.Empty : __request.QueryString["command"].Trim();
            //判断参数command是否为空，如果为空则抛出异常
            if (__command.Length == 0) throw new Exception(Resource.err_paramIsNotEmpty);



            //判断response格式
            //string __format = __request.QueryString["response"] == null ? "json" : __request.QueryString["response"].Trim();
            //switch (__format)
            //{
            //    case "json":
            //        break;
            //    case "xml":
            //        break;
            //    default:
            //        throw new Exception(Resource.err_paramResponse);
            //}
            //__context.Items.Add("response", __format);


            //非login命令参数验证
            if (__command != "login" && !Util.CheckRequestFromLocal())
            {
                //判断apikey
                string __apikey_3 = __request.QueryString["apikey"] == null ? string.Empty : __request.QueryString["apikey"].Trim();
                if (__apikey_3.Length == 0) throw new Exception(Resource.err_paramApiKeyIsNotEmpty);

                //判断signature
                string __signature_3 = __request.QueryString["signature"] == null ? string.Empty : __request.QueryString["signature"].Trim();
                if (__signature_3.Length == 0) throw new Exception(Resource.err_paramSignatureIsNotEmpty);
            }

            //获取Cmd实例
//            ICmd __cmd = CmdManager.INSTANCE.GetCmd(__command);
//            //公开命令判断
//            if (__cmd.Access == AccessLevel.PRIVATE) throw new Exception(Resource.err_noPublicCmd);

//            Type __type = __cmd.GetType();

//            //获取方法引用
//            MethodInfo __methodInfo = __type.GetMethod("Execute");

//            //创建命令参数组
//            Dictionary<string, string> __parameters = new Dictionary<string, string>();
//            __context.Items.Add("parameters", __parameters);

//            foreach (ParameterAttribute __paramAttr_3 in __methodInfo.GetCustomAttributes(typeof(ParameterAttribute), false))
//            {
//                //获取表单参数名称
//                string __paramName_4 = __paramAttr_3.Name;

//                //获取表单参数对象（*正式使用需要改成form方式*）
//#if DEBUG
//                object __param_4 = __request[__paramName_4];
//#else
//                object __param_4 = __request.Form[__paramName_4];
//#endif

//                //表单参数必填项验证
//                if (__paramAttr_3.Required && __param_4 == null) throw new Exception(string.Format(Resource.err_formParamRequire, __paramName_4));

//                //获取表单参数值
//                string __paramValue_4 = __param_4.ToString().Trim();

//                //对表单参数值进行正则表达式验证
//                if (__paramAttr_3.Regexp != string.Empty && !Regex.IsMatch(__paramValue_4, __paramAttr_3.Regexp))
//                {
//                    __context.Items["result"] += ",\"tip\":\"" + __paramAttr_3.RegexpInfo + "\"";
//                    throw new Exception(string.Format(Resource.err_paramValueRegex, __paramName_4));
//                }

//                //反射为命令参数赋值
//                __parameters.Add(__paramName_4, __paramValue_4);
//            }

//            __context.Items.Add("cmd", __cmd);


            ////循环属性字段
            //foreach (PropertyInfo __propInfo_3 in __type.GetProperties())
            //{
            //    ParameterAttribute __paramAttr_3 = (ParameterAttribute)Attribute.GetCustomAttribute(__propInfo_3, typeof(ParameterAttribute));

            //    //获取表单参数名称
            //    string __paramName_4 = __paramAttr_3.Name;

            //    //获取表单参数对象
            //    object __param_4 = __request.Form[__paramName_4];

            //    //表单参数必填项验证
            //    if (__paramAttr_3.Required && __param_4 == null) throw new Exception(string.Format(Resource.err_formParamRequire, __paramName_4));

            //    //获取表单参数值
            //    string __paramValue_4 = __param_4.ToString().Trim();

            //    //对表单参数值进行正则表达式验证
            //    if (__paramAttr_3.Regex != string.Empty) throw new Exception(string.Format(Resource.err_paramValueRegex, __paramName_4));

            //    //反射为命令参数赋值
            //    __type.GetProperty(__propInfo_3.Name).SetValue(__cmd, __paramValue_4, null);
            //}
        }


        #region IHttpModule 成员
        public void Dispose() { }

        public void Init(HttpApplication application)
        {
            application.AcquireRequestState += new EventHandler(Application_BeginRequest);
        }
        #endregion
    }
}
