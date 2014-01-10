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

using Foreworld.Cmd.BookManage.BookType;

namespace Foreworld.Cmd.BookManage.Test
{
    [TestFixture]
    class TestBookCmd
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestBookCmd));

        [SetUp]
        public void Setup()
        {
            log4net.Config.XmlConfigurator.Configure();
            MockHttpContext.Init();

            String __session = "{'id':1}";
            Object __obj = JavaScriptConvert.DeserializeObject(__session);
            HttpContext.Current.Items["userInfo"] = __obj;
        }

        #region 测试列表

        [Test]
        public void test_List()
        {
            _log.Debug("test");
        }
        #endregion
    }
}
