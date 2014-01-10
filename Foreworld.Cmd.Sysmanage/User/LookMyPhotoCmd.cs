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
    [Implementation("lookMyPhoto", Description = "看我的照片", Version = "1.0.0.0")]
    class LookMyPhotoCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LookMyPhotoCmd));

        public override Category Category
        {
            get { return Category.FILE; }
        }

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            __logInfo.Msg = "[看我的照片]";
            _log.Debug(__logInfo);

            #region SQL参数
            SqlParameter[] __sps = { new SqlParameter("@id", SqlDbType.Int) };

            __sps[0].Value = @parameter.UserInfo["id"];

            __logInfo.Msg = __sps[0] + ": " + __sps[0].SqlValue;
            _log.Debug(__logInfo);

            __logInfo.Msg = Resource.LookMyPhotoCmd_Sql;
            _log.Debug(__logInfo);
            #endregion

            #region 执行SQL
            object __picObj = null;

            SqlDataReader __dr = null;
            try
            {
                __dr = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, Resource.LookMyPhotoCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Msg = Resource.LookMyPhotoCmd_Err;
                _log.Error(__logInfo, @ex);
            }

            if (__dr == null) return Util.ExceptionLog(Resource.LookMyPhotoCmd_Err);

            if (__dr.HasRows)
            {
                while (__dr.Read())
                {
                    __picObj = __dr["photo"];
                }
            }

            __dr.Close();
            __dr.Dispose();
            __dr = null;

            #region 图片对象存入上下文
            @parameter.HttpContext.Items["pic"] = __picObj;
            #endregion

            #endregion

            string __result = "{'size':'1'}";

            __logInfo.Msg = __result;
            _log.Debug(__logInfo);

            return __result;
        }
    }
}

