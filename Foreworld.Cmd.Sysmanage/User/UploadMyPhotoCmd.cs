#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

using log4net;
using Newtonsoft.Json;

using Foreworld.Cmd;
using Foreworld.Db;
using Foreworld.Utils;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.User
{
    [Implementation("uploadMyPhoto", Description = "上传我的照片", Version = "1.0.0.0")]
    class UploadMyPhotoCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(UploadMyPhotoCmd));

        public override Category Category
        {
            get { return Category.FILE; }
        }

        //文件后缀
        private static readonly string _suffix = "|.jpg|.gif|.png|.bmp|";
        //默认最大4M
        private static readonly int _maxFileLen = 1024 * 4000;

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            __logInfo.Msg = "[上传我的照片]";
            _log.Debug(__logInfo);

            #region 流转换并验证
            HttpFileCollection __files = @parameter.HttpContext.Request.Files;

            if (__files.Count == 0) return Util.ExceptionLog(Resource.UploadMyPhotoCmd_noChoose_Err);

            HttpPostedFile __uploadFile = __files[0];

            string __filetype = this.CheckFileSuffix(__uploadFile.FileName);
            if (__filetype == string.Empty) return Util.ExceptionLog(Resource.UploadMyPhotoCmd_suffix_Err);

            int __contentLength = __uploadFile.ContentLength;

            if (__contentLength > _maxFileLen) return Util.ExceptionLog(Resource.UploadMyPhotoCmd_fileLen_Err);


            byte[] __fileBytes = new byte[__contentLength];
            Stream __fileStream = __uploadFile.InputStream;
            __fileStream.Read(__fileBytes, 0, __contentLength);
            #endregion

            #region SQL参数
            List<SqlParameter> __sps = new List<SqlParameter>();

            SqlParameter __sp = null;

            __sp = new SqlParameter("@id", SqlDbType.Int);
            __sp.Value = @parameter.UserInfo["id"];
            __sps.Add(__sp);

            __sp = new SqlParameter("@photo", SqlDbType.Image);
            __sp.Value = __fileBytes;
            __sps.Add(__sp);

            __sp = new SqlParameter("@phototype", SqlDbType.VarChar, 10);
            __sp.Value = __filetype;
            __sps.Add(__sp);

            __logInfo.Msg = Resource.UploadMyPhotoCmd_Sql;
            _log.Debug(__logInfo);
            #endregion

            #region 执行SQL
            int __size = 0;

            try
            {
                __size = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, Resource.UploadMyPhotoCmd_Sql, __sps.ToArray());
            }
            catch (Exception @ex)
            {
                __logInfo.Msg = Resource.UploadMyPhotoCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            //找不到记录
            if (__size == 0) return Util.ExceptionLog(Resource.UploadMyPhotoCmd_Err);
            #endregion

            string __result = "{'size':'" + __size + "'}";

            __logInfo.Msg = __result;
            _log.Debug(__logInfo);

            __logInfo.Msg = Resource.UploadMyPhotoCmd;
            _log.Info(__logInfo);

            return __result;
        }

        /// <summary>
        /// 判断文件后缀是否合法
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string CheckFileSuffix(string @fileName)
        {
            int __idx = @fileName.LastIndexOf(".");
            if (__idx == -1) return string.Empty;
            string __suffix = @fileName.Substring(__idx).ToLower();
            return _suffix.IndexOf("|" + __suffix + "|") > -1 ? __suffix : string.Empty;
        }
    }
}

