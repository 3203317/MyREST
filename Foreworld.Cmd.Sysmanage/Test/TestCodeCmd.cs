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

using Foreworld.Cmd.Sysmanage.Code;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.Test
{
    [TestFixture]
    class TestCodeCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestCodeCmd));

        [SetUp]
        public void Setup()
        {
            log4net.Config.XmlConfigurator.Configure();
            MockHttpContext.Init();

            String __session = "{'id':1}";
            Object __obj = JavaScriptConvert.DeserializeObject(__session);
            HttpContext.Current.Items["userInfo"] = __obj;
        }

        private void testAdd()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("code", "TEST01");
            __params.Add("codetypeid", "SEX");
            __params.Add("p_code", "0");
            __params.Add("codename", "测试性别");
            __params.Add("codedesc", "测试性别");
            __params.Add("sort", "4");

            AddCodeCmd __cmd = new AddCodeCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();
        }

        private void testEdit()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("code", "TEST01");
            __params.Add("codetypeid", "SEX");
            __params.Add("codename", "测试性别121");
            __params.Add("codedesc", "测试性别232");
            __params.Add("sort", "343");

            UpdateCodeCmd __cmd = new UpdateCodeCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();
        }

        private void testDel()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("code", "TEST01");
            __params.Add("codetypeid", "SEX");

            DeleteCodeCmd __cmd = new DeleteCodeCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();
        }

        [Test]
        public void test_List()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("codetypeid", "SEX");

            ListCodesCmd __cmd = new ListCodesCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();
        }

        [Test]
        public void test_CRUD()
        {
            TestCodeCmd __t = new TestCodeCmd();
            __t.testAdd();

            __t.test_List();

            __t.testEdit();

            __t.test_List();

            __t.testDel();

            __t.test_List();
        }
    }
}
