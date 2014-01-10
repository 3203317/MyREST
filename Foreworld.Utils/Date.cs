using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Utils
{
    public class Date
    {
        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="sDateTime"></param>
        /// <param name="eDateTime"></param>
        /// <returns>秒</returns>
        public static double TimeDiff(string sDateTime, string eDateTime)
        {
            DateTime __sDateTime = Convert.ToDateTime(sDateTime);
            DateTime __eDateTime = Convert.ToDateTime(eDateTime);

            TimeSpan __ts = __eDateTime - __sDateTime;
            return __ts.TotalSeconds;
        }

        /// <summary>
        /// 验证字符串是否是日期类型
        /// </summary>
        /// <param name="time">日期字符串</param>
        /// <returns></returns>
        public static bool ValidateTimeType(string time)
        {
            return false;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("foreworld.net");
        }
    }
}
