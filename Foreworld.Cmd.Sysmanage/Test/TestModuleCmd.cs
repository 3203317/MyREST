//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Collections;
//using System.Web;
//using System.Security.Cryptography;
//using System.IO;
//using System.Net;

//using NUnit.Framework;
//using Newtonsoft.Json;
//using log4net;

//using Foreworld.Cmd.Privilege.Model;
//using Foreworld.Log;

//namespace Foreworld.Cmd.Sysmanage.Test
//{
//    [TestFixture]
//    class TestModuleCmd
//    {
//        private static readonly ILog _log = LogManager.GetLogger(typeof(TestModuleCmd));

//        [SetUp]
//        public void Setup()
//        {
//            log4net.Config.XmlConfigurator.Configure();
//            MockHttpContext.Init();

//            String __session = "{'id':1}";
//            Object __obj = JavaScriptConvert.DeserializeObject(__session);
//            HttpContext.Current.Items["userInfo"] = __obj;
//        }

//        private string testAdd()
//        {
//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
//            LogInfo __logInfo = new LogInfo();

//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("p_id", "0");
//            __params.Add("modulename", "测试模块");
//            __params.Add("icon", "xxx.gif");
//            __params.Add("href", "xxx.html");
//            __params.Add("sort", "4");

//            AddModuleCmd __cmd = new AddModuleCmd();

//            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

//            JavaScriptObject __jso = (JavaScriptObject)JavaScriptConvert.DeserializeObject(__result);
//            JavaScriptArray __jsa = (JavaScriptArray)__jso["items"];
//            JavaScriptObject __obj = (JavaScriptObject)__jsa[0];


//            return __obj["id"].ToString();
//        }

//        private void testEdit(string @id)
//        {
//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
//            LogInfo __logInfo = new LogInfo();


//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("id", @id);
//            __params.Add("modulename", "测试模块121");
//            __params.Add("icon", "xxx232.gif");
//            __params.Add("href", "xxx343.html");
//            __params.Add("sort", "454");

//            UpdateModuleCmd __cmd = new UpdateModuleCmd();

//            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();
//        }

//        private void testDel(string @id)
//        {
//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
//            LogInfo __logInfo = new LogInfo();

//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("ids", @id);

//            DeleteModuleCmd __cmd = new DeleteModuleCmd();

//            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();
//        }

//        [Test]
//        public void test_List()
//        {
//            ListModulesCmd __cmd = new ListModulesCmd();

//            Dictionary<string, string> __params = new Dictionary<string, string>();
//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
//            LogInfo __logInfo = new LogInfo();

//            __params.Add("p_id", "0");

//            __cmd.Execute(new Parameter(Response.JSON, null, __logInfo, __params));
//        }

//        [Test]
//        public void test_CRUD()
//        {

//            TestModuleCmd __t = new TestModuleCmd();
//            string __id = __t.testAdd();

//            __t.test_List();

//            __t.testEdit(__id);

//            __t.test_List();

//            __t.testDel(__id);

//            __t.test_List();
//        }

//        [Test]
//        public void test_findModOpts()
//        {

//            ListModOptsCmd __cmd = new ListModOptsCmd();

//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("moduleid", "5");

//            __cmd.Execute(new Parameter(Response.JSON, null, __params));
//        }
//    }
//}
