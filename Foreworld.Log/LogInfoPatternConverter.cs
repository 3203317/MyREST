using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using log4net.Layout.Pattern;
using log4net.Core;

namespace Foreworld.Log
{
    class LogInfoPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter @writer, LoggingEvent @loggingEvent)
        {
            if (Option != null)
            {
                // Write the value for the specified key
                WriteObject(@writer, @loggingEvent.Repository, LookupProperty(Option, @loggingEvent));
            }
            else
            {
                // Write all the key value pairs
                WriteDictionary(@writer, @loggingEvent.Repository, @loggingEvent.GetProperties());
            }

            //if (Option != null)
            //{
            //    // Write the value for the specified key
            //    WriteObject(writer, loggingEvent.Repository, loggingEvent.LookupProperty(Option));
            //}
            //else
            //{
            //    // Write all the key value pairs
            //    WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            //}
        }

        /// <summary>
        /// 通过反射获取传入的日志对象的某个属性的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="loggingEvent"></param>
        /// <returns></returns>
        private object LookupProperty(string @property, LoggingEvent @loggingEvent)
        {
            object __propertyValue = string.Empty;
            PropertyInfo __propertyInfo = @loggingEvent.MessageObject.GetType().GetProperty(@property);
            if (__propertyInfo != null) __propertyValue = __propertyInfo.GetValue(@loggingEvent.MessageObject, null);
            return __propertyValue;
        }
    }
}
