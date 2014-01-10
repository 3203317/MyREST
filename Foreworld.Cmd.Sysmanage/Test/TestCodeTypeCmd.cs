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

using Foreworld.Cmd.Sysmanage.CodeType;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.Test
{
    [TestFixture]
    class TestCodeTypeCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestCodeTypeCmd));

        [SetUp]
        public void Setup()
        {
            log4net.Config.XmlConfigurator.Configure();
            MockHttpContext.Init();

            String __session = "{'id':1}";
            Object __obj = JavaScriptConvert.DeserializeObject(__session);
            HttpContext.Current.Items["userInfo"] = __obj;
        }

        private string testAdd()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            string __id = "TESTSEX";

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("id", __id);
            __params.Add("codetypedesc", "测试性别");

            AddCodeTypeCmd __cmd = new AddCodeTypeCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

            return __id;
        }

        [Test]
        public void test_List()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            ListCodeTypesCmd __cmd = new ListCodeTypesCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();
        }

        private void testEdit(string @id)
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("id", @id);
            __params.Add("codetypedesc", "测试性别121");

            UpdateCodeTypeCmd __cmd = new UpdateCodeTypeCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

        }

        private void testDel(string @id)
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("id", @id);

            DeleteCodeTypeCmd __cmd = new DeleteCodeTypeCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

        }

        [Test]
        public void test_CRUD()
        {
            TestCodeTypeCmd __t = new TestCodeTypeCmd();
            string __id = __t.testAdd();

            __t.test_List();

            __t.testEdit(__id);

            __t.test_List();

            __t.testDel(__id);

            __t.test_List();
        }
    }
}
