#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using log4net;
using Newtonsoft.Json;

using Foreworld.Cmd;
using Foreworld.Db;
using Foreworld.Utils;
using Foreworld.Log;

namespace Foreworld.Cmd.Build.Page
{
    [Implementation("listPageTree", Description = "页面树列表", Version = "1.0.0.0")]
    class ListPageTreeCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListPageTreeCmd));

        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;

            #region SQL参数
#if DEBUG
            __logInfo.Msg = Resource.ListPageTreeCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            DataSet __ds = null;

            #region DataSet
            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.ListPageTreeCmd_Sql);
            }
            catch (Exception @ex)
            {
                __logInfo.Msg = Resource.ListPageTreeCmd_Err;
                _log.Error(__logInfo, @ex);
            }
            #endregion

            #region 判断ds是否为空
            if (__ds == null) return Util.ExceptionLog(Resource.ListPageTreeCmd_Err);
            #endregion

#if DEBUG
            __logInfo.Msg = __ds.GetXml();
            _log.Debug(__logInfo);
#endif
            return __ds;
        }
    }
}
