using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;

using Newtonsoft.Json;

namespace Foreworld.Cmd
{
    public enum ParameterType
    {
        BOOLEAN,
        DATE,
        FLOAT,
        INTEGER,
        SHORT,
        LIST,
        LONG,
        OBJECT,
        MAP,
        STRING,
        TZDATE
    }

    public abstract class AbstractBaseCmd : ICmd
    {

        public virtual AccessLevel Access
        {
            get { return AccessLevel.PROTECTED; }
        }

        public virtual Category Category
        {
            get { return Category.INFO; }
        }

        public virtual string ConnectionString
        {
            get { return ConfigurationSettings.AppSettings["connectionString"]; }
        }

        public abstract object Execute(Parameter @parameter);

        protected string GetData(HttpContext @context)
        {
            string __data = @context.Request.Form["data"];
            return __data;
        }

        public void Destroy()
        {

        }
    }
}
