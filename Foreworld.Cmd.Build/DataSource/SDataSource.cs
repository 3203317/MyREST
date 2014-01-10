using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd.Build.DataSource
{
    [Serializable]
    public class SDataSource
    {
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string DsName { get; set; }

        /// <summary>
        /// 附加参数
        /// </summary>
        public string Params { get; set; }
    }
}
