using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Foreworld.Db
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbType
    {
        SQLSERVER2008,
        ORACLE,
        ACCESS,
        MYSQL
    }

    public class DbFactory
    {
        public static string Test()
        {

            DataSet ds = SqlHelper.ExecuteDataSet("Data Source=.;Initial Catalog=sysmanage2;Persist Security Info=True;User ID=sa;Pwd=123a", CommandType.Text, "select * from sysmanage_user");



            return ",'test':'" + ds.Tables[0].Rows[0][4] + "'";
        }

    }
}