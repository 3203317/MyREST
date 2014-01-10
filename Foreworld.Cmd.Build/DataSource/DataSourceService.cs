using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Build.DataSource
{
    public interface DataSourceService : IService
    {
        ResultMapper findDSList();

        ResultMapper loadDSData(Pagination @page, SDataSource @search);
    }
}
