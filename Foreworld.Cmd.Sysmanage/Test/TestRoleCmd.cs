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

using Foreworld.Cmd.Sysmanage.Role;
using Foreworld.Log;

namespace Foreworld.Cmd.Sysmanage.Test
{
    [TestFixture]
    class TestRoleCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestRoleCmd));

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


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("rolegroupid", "ADMINS");
            __params.Add("rolename", "TEST角色_01");
            __params.Add("roledesc", "描述");
            __params.Add("startime", "123");
            __params.Add("endtime", "456");

            AddRoleCmd __cmd = new AddRoleCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

            JavaScriptObject __jso = (JavaScriptObject)JavaScriptConvert.DeserializeObject(__result);
            JavaScriptArray __jsa = (JavaScriptArray)__jso["items"];
            JavaScriptObject __obj = (JavaScriptObject)__jsa[0];


            return __obj["id"].ToString();
        }

        private void testEdit(string @id)
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("id", @id);
            __params.Add("rolename", "TEST角色_01");
            __params.Add("roledesc", "描述2");
            __params.Add("startime", "123456");
            __params.Add("endtime", "456789");

            UpdateRoleCmd __cmd = new UpdateRoleCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

        }

        private void testDel(string @id)
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("ids", @id);

            DeleteRoleCmd __cmd = new DeleteRoleCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

        }

        [Test]
        public void test_List()
        {

            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();

            //__params.Add("rolegroupid", "ADMINS");
            //__params.Add("username", "w");


            ListRolesCmd __cmd = new ListRolesCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();
        }

        [Test]
        public void test_CRUD()
        {
            TestRoleCmd __t = new TestRoleCmd();
            string __id = __t.testAdd();

            __t.test_List();

            __t.testEdit(__id);

            __t.test_List();

            __t.testDel(__id);

            __t.test_List();
        }



        private string testEmpower()
        {

            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("roleid", "0");
            __params.Add("modoptid", "0");

            EmpowerRoleCmd __cmd = new EmpowerRoleCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();


            return "";
        }

        private void testUnEmpower()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
            LogInfo __logInfo = new LogInfo();

            Dictionary<string, string> __params = new Dictionary<string, string>();


            __params.Add("roleid", "0");
            __params.Add("modoptid", "0");

            UnEmpowerRoleCmd __cmd = new UnEmpowerRoleCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, __params)).ToString();

        }


        [Test]
        public void test_Empower()
        {
            _log.Debug("授权角色功能测试");
            TestRoleCmd __t = new TestRoleCmd();
            string __id = __t.testEmpower();

            __t.testUnEmpower();
        }
    }
}
