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

using Foreworld.Cmd.Sysmanage.Org;

namespace Foreworld.Cmd.Sysmanage.Test
{
    [TestFixture]
    class TestOrgCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestOrgCmd));

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


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("parentid", "0");
            __params.Add("orgname", "测试组织");
            __params.Add("orgdesc", "测试组织描述");
            __params.Add("orgtypeid", "0");
            __params.Add("sort", "4");

            AddOrgCmd __cmd = new AddOrgCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params)).ToString();

            JavaScriptObject __jso = (JavaScriptObject)JavaScriptConvert.DeserializeObject(__result);
            JavaScriptArray __jsa = (JavaScriptArray)__jso["items"];
            JavaScriptObject __obj = (JavaScriptObject)__jsa[0];


            return __obj["id"].ToString();
        }

        private void testEdit(string @id)
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("id", @id);
            __params.Add("orgname", "测试组织121");
            __params.Add("orgdesc", "测试组织描述232");
            __params.Add("orgtypeid", "0");
            __params.Add("sort", "454");

            UpdateOrgCmd __cmd = new UpdateOrgCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params)).ToString();
        }

        private void testDel(string @id)
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("ids", @id);

            DeleteOrgCmd __cmd = new DeleteOrgCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params)).ToString();
        }

        [Test]
        public void test_List()
        {
            ListOrgsCmd __cmd = new ListOrgsCmd();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("parentid", "0");

            string __result = __cmd.Execute(new Parameter(Response.JSON, null, __params)).ToString();
        }

        [Test]
        public void test_CRUD()
        {
            _log.Debug("组织CRUD测试");

            TestOrgCmd __t = new TestOrgCmd();
            string __id = __t.testAdd();

            __t.test_List();

            __t.testEdit(__id);

            __t.test_List();

            __t.testDel(__id);

            __t.test_List();
        }
    }
}
