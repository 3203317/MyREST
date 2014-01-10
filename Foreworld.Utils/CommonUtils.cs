using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Foreworld.Utils
{
    class CommonUtils
    {
        /// <summary>
        /// 用于字符串和枚举类型的转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T EnumParse<T>(string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch
            {
                throw new Exception("传入的值与枚举值不匹配.");
            }
        }

        /// <summary>
        /// 根据传入的Key获取配置文件中的Value值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValueByKey(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].Trim();
            }
            catch
            {
                throw new Exception("web.config中 key=\"" + key + "\" 未配置或配置错误.");
            }
        }

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(Object value)
        {
            if (value == null) return true;
            if (String.IsNullOrEmpty(value.ToString().Trim())) return true;
            return false;
        }
    }
}
