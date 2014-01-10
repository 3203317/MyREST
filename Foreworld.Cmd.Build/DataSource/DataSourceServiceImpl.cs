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

namespace Foreworld.Cmd.Build.DataSource
{
    class DataSourceServiceImpl : BaseService, DataSourceService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DataSourceServiceImpl));

        public ResultMapper findDSList()
        {
            ResultMapper __mapper = new ResultMapper();


            DataSet __ds = null;

            #region DataSet
            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, Resource.ListDataSourcesCmd_Sql);
            }
            catch (Exception @ex)
            {
                _log.Error(@ex);
            }
            #endregion

            __mapper.Success = true;
            return __mapper;
        }


        public ResultMapper loadDSData(Pagination @page, SDataSource @search)
        {
            ResultMapper __mapper = new ResultMapper();




            __mapper.Success = true;
            return __mapper;
        }
    }
}
