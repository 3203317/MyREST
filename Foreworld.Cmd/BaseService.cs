using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Foreworld.Cmd
{
    public abstract class BaseService
    {
        private string _connectionString = ConfigurationSettings.AppSettings["connectionString"];

        public virtual string ConnectionString
        {
            get { return _connectionString; }
        }
    }
}
