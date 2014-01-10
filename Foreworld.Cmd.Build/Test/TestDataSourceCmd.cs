using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Net;

using NUnit.Framework;
using Newtonsoft.Json;
using log4net;

using Foreworld.Cmd.Build.DataSource;
using Foreworld.Log;

namespace Foreworld.Cmd.Build.Test
{
    [TestFixture]
    class TestDataSourceCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestDataSourceCmd));

        [SetUp]
        public void Setup()
        {
            log4net.Config.XmlConfigurator.Configure();
            MockHttpContext.Init();

            String __session = "{'id':1}";
            Object __obj = JavaScriptConvert.DeserializeObject(__session);
            HttpContext.Current.Items["userInfo"] = __obj;
        }

        [Test]
        public void test_List()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            ListDataSourcesCmd __cmd = new ListDataSourcesCmd();

            __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params));
        }

        [Test]
        public void test_DataSource()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("name", "5DE6C513E0FB4D2E8C90982C3B117193");
            __params.Add("params", "and rolename like '%员%'");
            __params.Add("current", "1");
            __params.Add("pagesize", "1");

            DataSourceCmd __cmd = new DataSourceCmd();

            __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params));
        }

        [Test]
        public void test_CRUD()
        {
            TestDataSourceCmd __t = new TestDataSourceCmd();

            __t.test_List();
        }
    }
}
