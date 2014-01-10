using System;
using System.Web;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;

namespace Foreworld.Rest.Api
{
    class Api : BaseApi
    {
        public override void ProcessRequest(HttpContext @context)
        {
            HttpResponse __response = @context.Response;
            __response.ClearHeaders();
            __response.AddHeader("Email", "huangxin@foreworld.net");
            __response.Charset = "UTF-8";

            object __typeObj = @context.Request.QueryString["type"];
            string __type = __typeObj == null ? null : __typeObj.ToString().Trim();

            switch (__type)
            {
                case null:
                    ApInterceptorManager.INSTANCE.Run(@context);
                    ProcessApi(@context);
                    break;
                case "download":
                    DownloadInterceptorManager.INSTANCE.Run(@context);
                    ProcessDownload(@context);
                    break;
                case "fileUpload":
                    FileUploadInterceptorManager.INSTANCE.Run(@context);
                    ProcessFileUpload(@context);
                    break;
                case "pic":
                    PicInterceptorManager.INSTANCE.Run(@context);
                    ProcessPic(@context);
                    break;
                default:
                    ProcessDefault(@context);
                    break;
            }

            __response.End();
        }

        private void ProcessApi(HttpContext @context)
        {
            JavaScriptObject __jsObj = (JavaScriptObject)JavaScriptConvert.DeserializeObject(@context.Items["result"].ToString());
            __jsObj.Add(AssemblyCompany, Assembly.GetExecutingAssembly().GetName().Version.ToString());
            HttpResponse __response = @context.Response;
            __response.ContentType = "text/plain";
            __jsObj.Add("processed", (DateTime.Now - _startTime).TotalSeconds.ToString());
            string __result = JavaScriptConvert.SerializeObject(__jsObj);
            __response.Write(__result);
        }

        private void ProcessDownload(HttpContext @context)
        {
            HttpResponse __response = @context.Response;

            JavaScriptObject __jsObj = (JavaScriptObject)JavaScriptConvert.DeserializeObject(@context.Items["result"].ToString());

            object __fileObj = @context.Items["file"];

            if (__fileObj == null)
            {
                __jsObj.Add(AssemblyCompany, Assembly.GetExecutingAssembly().GetName().Version.ToString());
                __response.ContentType = "text/plain";
                __jsObj.Add("processed", (DateTime.Now - _startTime).TotalSeconds.ToString());
                string __result_3 = JavaScriptConvert.SerializeObject(__jsObj);
                __response.Write(__result_3);
            }
            else
            {
                string __filetype_3 = __jsObj["filetype"].ToString();
                string __filename_3 = __jsObj["filename"].ToString();

                string __browser_3 = @context.Request.UserAgent.ToUpper();
                if (!__browser_3.Contains("FIREFOX")) __filename_3 = HttpUtility.UrlEncode(__filename_3, Encoding.UTF8);

                __response.Clear();
                __response.Expires = 0;
                __response.AppendHeader("Content-Disposition", "attachment;filename=" + __filename_3 + __filetype_3);
                __response.ContentEncoding = Encoding.GetEncoding("UTF-8");

                __response.ContentType = GetContentType(__filetype_3);

                __response.BinaryWrite((byte[])__fileObj);
            }
        }

        private void ProcessFileUpload(HttpContext @context)
        {
            JavaScriptObject __jsObj = (JavaScriptObject)JavaScriptConvert.DeserializeObject(@context.Items["result"].ToString());
            __jsObj.Add(AssemblyCompany, Assembly.GetExecutingAssembly().GetName().Version.ToString());
            HttpResponse __response = @context.Response;
            __response.ContentType = "text/plain";
            __jsObj.Add("processed", (DateTime.Now - _startTime).TotalSeconds.ToString());
            string __result = JavaScriptConvert.SerializeObject(__jsObj);
            __response.Write(__result);
        }

        private void ProcessPic(HttpContext @context)
        {
            HttpResponse __response = @context.Response;
            object __picObj = @context.Items["pic"];

            if (__picObj == null)
            {
                JavaScriptObject __jsObj_3 = (JavaScriptObject)JavaScriptConvert.DeserializeObject(@context.Items["result"].ToString());
                __jsObj_3.Add(AssemblyCompany, Assembly.GetExecutingAssembly().GetName().Version.ToString());
                __response.ContentType = "text/plain";
                __jsObj_3.Add("processed", (DateTime.Now - _startTime).TotalSeconds.ToString());
                string __result_3 = JavaScriptConvert.SerializeObject(__jsObj_3);
                __response.Write(__result_3);
            }
            else
            {
                __response.ContentType = __picObj as string;
                __response.BinaryWrite((byte[])__picObj);
            }
        }

        private void ProcessDefault(HttpContext @context)
        {
            @context.Response.Write("{\"" + AssemblyCompany + "\":\"" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\",\"processed\":\"" + (DateTime.Now - _startTime).TotalSeconds.ToString() + "\"}");
        }

        private static string GetContentType(string @fileType)
        {
            string __suffix;
            switch (@fileType)
            {
                case ".doc": __suffix = "application/vnd.ms-word"; break;
                case ".jpg": __suffix = "image/jpeg"; break;
                case ".txt": __suffix = "application/vnd.ms-word"; break;
                case ".tif||.tiff": __suffix = "text/plain"; break;
                case ".gif": __suffix = "image/gif"; break;
                case ".bmp": __suffix = "image/bmp"; break;
                case ".swf": __suffix = "application/x-shockwave-flash"; break;
                case ".zip": __suffix = "application/zip"; break;
                case ".sql": __suffix = "application/vnd.ms-word"; break;
                case ".dwg": __suffix = "application/autocad"; break;
                case ".pdf": __suffix = "application/pdf"; break;
                case ".ppt": __suffix = "appication/powerpoint"; break;
                default: __suffix = "text/html"; break;
            }
            return __suffix;
        }
    }
}
