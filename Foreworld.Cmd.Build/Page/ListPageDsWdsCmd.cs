﻿#define DEBUG
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
    [Implementation("listPageDsWds", Description = "页面数据源组件列表", Version = "1.0.0.0")]
    class ListPageDsWdsCmd : AbstractBaseCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ListPageDsWdsCmd));

        [Parameter("pagedsid", "页数据源ID", Required = true, Regexp = "^\\d{1,10}$", RegexpInfo = "支持数字")]
        public override object Execute(Parameter @parameter)
        {
            LogInfo __logInfo = @parameter.LogInfo;
#if DEBUG
            __logInfo.Msg = "[页面数据源组件列表]";
            _log.Debug(__logInfo);
#endif
            #region SQL参数
            SqlParameter[] __sps = { new SqlParameter("@pagedsid", SqlDbType.Int) };
            __sps[0].Value = @parameter.Parameters["pagedsid"];
#if DEBUG
            __logInfo.Msg = __sps[0] + ": " + __sps[0].SqlValue;
            _log.Debug(__logInfo);
            __logInfo.Msg = Resource.ListPageDsWdsCmd_Sql;
            _log.Debug(__logInfo);
#endif
            #endregion

            DataSet __ds = null;

            #region DataSet
            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.ListPageDsWdsCmd_Sql, __sps);
            }
            catch (Exception @ex)
            {
                __logInfo.Msg = Resource.ListPageDsWdsCmd_Err;
                _log.Error(__logInfo, @ex);
            }
            #endregion

            #region 判断ds是否为空
            if (__ds == null) return Util.ExceptionLog(Resource.ListPageDsWdsCmd_Err);
            #endregion

#if DEBUG
            __logInfo.Msg = __ds.GetXml();
            _log.Debug(__logInfo);
#endif
            return __ds;
        }
    }
}
