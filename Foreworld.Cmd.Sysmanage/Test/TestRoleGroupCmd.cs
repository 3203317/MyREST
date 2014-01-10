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

using Foreworld.Cmd.Sysmanage.RoleGroup;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.Test
{
    [TestFixture]
    class TestRoleGroupCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestRoleGroupCmd));

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


            ListRoleGroupsCmd __cmd = new ListRoleGroupsCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();
        }

        private string testAdd()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            string __id = "GUESTS";

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("id", __id);
            __params.Add("rolegroupdesc", "普通游客组");

            AddRoleGroupCmd __cmd = new AddRoleGroupCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

            return __id;
        }

        private void testDel(string @id)
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("id", @id);

            DeleteRoleGroupCmd __cmd = new DeleteRoleGroupCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

        }

        private void testEdit(string @id)
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("id", @id);
            __params.Add("rolegroupdesc", "测试角色组121");

            UpdateRoleGroupCmd __cmd = new UpdateRoleGroupCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

        }

        [Test]
        public void test_CRUD()
        {
            TestRoleGroupCmd __t = new TestRoleGroupCmd();
            string __id = __t.testAdd();

            __t.test_List();

            __t.testEdit(__id);

            __t.test_List();

            __t.testDel(__id);

            __t.test_List();
        }
    }
}
