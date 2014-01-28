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

using Foreworld.Log;

namespace Foreworld.Cmd.Blog.Test
{
    [TestFixture]
    class TestCategory
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TestCategory));

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void test_CRUD()
        {
            Console.WriteLine("Hello");
        }
    }
}
