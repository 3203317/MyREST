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
    [Implementation("downloadMyPhoto", Description = "下载我的照片", Version = "1.0.0.0")]
    class DownloadMyPhotoCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DownloadMyPhotoCmd));

        public override Category Category
        {
            get { return Category.FILE; }
        }

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            __logInfo.Msg = "[下载我的照片]";
            _log.Debug(__logInfo);

            #region SQL参数
            SqlParameter[] __sps = { new SqlParameter("@id", SqlDbType.Int) };

            __sps[0].Value = @parameter.UserInfo["id"];

            __logInfo.Msg = __sps[0] + ": " + __sps[0].SqlValue;
            _log.Debug(__logInfo);

            __logInfo.Msg = Resource.DownloadMyPhotoCmd_Sql;
            _log.Debug(__logInfo);
            #endregion

            #region 执行SQL
            object __picObj = null;
            string __filename = string.Empty;
            string __filetype = string.Empty;

            SqlDataReader __dr = null;
            try
            {
                __dr = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, Resource.DownloadMyPhotoCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Code = Resource.DownloadMyPhotoCmd_Err_Code;
                __logInfo.Msg = Resource.DownloadMyPhotoCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            if (__dr == null) return Util.ExceptionLog(Resource.DownloadMyPhotoCmd_Err);

            if (__dr.HasRows)
            {
                while (__dr.Read())
                {
                    __picObj = __dr["photo"];
                    __filename = __dr["username"] + "[" + __dr["firstname"] + __dr["lastname"] + "]";
                    __filetype = __dr["phototype"].ToString();
                }
            }

            __dr.Close();
            __dr.Dispose();
            __dr = null;

            if (__picObj == null) return Util.ExceptionLog(Resource.DownloadMyPhotoCmd_Err);

            #region 图片对象存入上下文
            @parameter.HttpContext.Items["file"] = __picObj;
            #endregion

            #endregion

            #region JSON对象
            StringWriter __sw = new StringWriter();
            JsonWriter __jw = new JsonWriter(__sw);

#if DEBUG
            __jw.Formatting = Formatting.Indented;
#endif

            __jw.WriteStartObject();

            __jw.WritePropertyName("size");
            __jw.WriteValue("1");

            __jw.WritePropertyName("filename");
            __jw.WriteValue(__filename);

            __jw.WritePropertyName("filetype");
            __jw.WriteValue(__filetype);

            __jw.WriteEndObject();

            __jw.Flush();

            string __result = __sw.GetStringBuilder().ToString();

            __jw.Close();
            __sw.Close();
            __sw.Dispose();
            #endregion

            __logInfo.Msg = __result;
            _log.Debug(__logInfo);

            return __result;
        }
    }
}

