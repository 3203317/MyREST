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

using Foreworld.Cmd.Sysmanage.MyUrl;

namespace Foreworld.Cmd.Sysmanage.Test
{
    [TestFixture]
    class TestMyUrlCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestMyUrlCmd));

        [SetUp]
        public void Setup()
        {
            log4net.Config.XmlConfigurator.Configure();
            MockHttpContext.Init();

            String __session = "{'id':2}";
            Object __obj = JavaScriptConvert.DeserializeObject(__session);
            HttpContext.Current.Items["userInfo"] = __obj;
        }

        #region 测试查询

        [Test]
        public void test_List()
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];

            ListMyUrlsCmd __cmd = new ListMyUrlsCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, null)).ToString();
        }
        #endregion

        #region 测试CRUD


        private string testAdd()
        {

            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];


            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("url", "http://www.test000.com");
            __params.Add("memo", "背黑族");
            __params.Add("ispublic", "2");

            AddMyUrlCmd __cmd = new AddMyUrlCmd();

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

            __params.Add("url", "http://www.test.com");
            __params.Add("memo", "test");
            __params.Add("ispublic", "1");

            UpdateMyUrlCmd __cmd = new UpdateMyUrlCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params)).ToString();
        }

        private void testDel(string @id)
        {
            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];

            Dictionary<string, string> __params = new Dictionary<string, string>();

            __params.Add("ids", @id);

            DeleteMyUrlCmd __cmd = new DeleteMyUrlCmd();

            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params)).ToString();
        }

        [Test]
        public void test_CRUD()
        {
            _log.Debug("我的网址功能测试");
            TestMyUrlCmd __t = new TestMyUrlCmd();
            string __id = __t.testAdd();

            __t.test_List();

            __t.testEdit(__id);

            __t.test_List();

            __t.testDel(__id);

            __t.test_List();
        }
        #endregion
    }
}
