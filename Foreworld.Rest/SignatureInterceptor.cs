using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.IO;
using System.Security.Cryptography;

using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Foreworld.Utils;
using Foreworld.Cmd;

namespace Foreworld.Rest
{
    public class SignatureInterceptor : Interceptor
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SignatureInterceptor));

        public override void RequestInterceptor(HttpContext @context)
        {
            HttpRequest __request = @context.Request;
            //获取Cmd实例
            //ICmd __cmd = CmdManager.INSTANCE.GetCmd(__request.QueryString["command"].Trim());

            //if (__cmd.Access == AccessLevel.PROTECTED)
            //{
            //    string __formParams_3 = GetFormParamsJoin(__request);
            //    string __secretkey_3 = ((JavaScriptObject)@context.Items["userInfo"])["secretkey"].ToString();
            //    string __signature_3 = __request.QueryString["signature"].Trim();

            //    if (VerifyUserSignature(__formParams_3, __secretkey_3, __signature_3))
            //    {
            //        successor.RequestInterceptor(@context);
            //    }
            //    else
            //    {
            //        Util.ExceptionLog(@context, Status.FAILURE, Resource.err_signatureValidate);
            //    }
            //}
            //else
            //{
            //    successor.RequestInterceptor(@context);
            //}
        }

        /// <summary>
        /// 表单字符串连接
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetFormParamsJoin(HttpRequest @request)
        {
            Dictionary<string, string> __params = new Dictionary<string, string>();

            /* Form */
            foreach (string __key_3 in @request.Form.AllKeys)
            {
                string __val_4 = @request.Form[__key_3].Trim();
                __params.Add(__key_3, __key_3 + "=" + (__val_4.Length < 51 ? __val_4 : __val_4.Substring(0, 50)));
            }

            /* Query */
            foreach (string __key_3 in @request.QueryString.AllKeys)
            {
                string __val_4 = @request.QueryString[__key_3].Trim();
                __params.Add(__key_3, __key_3 + "=" + (__val_4.Length < 51 ? __val_4 : __val_4.Substring(0, 50)));
            }

            __params.Remove("signature");

            /* 创建数组 */
            string[] __paramsArr = (new List<string>(__params.Values)).ToArray();
            Array.Sort(__paramsArr);

            /* 地址参数连接字符串 */
            string __paramsJoin = string.Join("&", __paramsArr);
            return __paramsJoin;
        }

        /// <summary>
        /// 验证用户Signature
        /// </summary>
        /// <param name="formParams"></param>
        /// <param name="secretkey"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        private bool VerifyUserSignature(string @formParams, string @secretkey, string @signature)
        {
            HMACSHA1 __hmacSha1 = new HMACSHA1(Encoding.Default.GetBytes(@secretkey));
            __hmacSha1.Initialize();
            byte[] __hmac = __hmacSha1.ComputeHash(Encoding.Default.GetBytes(@formParams.ToLower()));
            string __base64 = Convert.ToBase64String(__hmac);
            string __signature = HttpUtility.UrlEncode(__base64, Encoding.UTF8);

            return __signature == @signature;
        }
    }
}