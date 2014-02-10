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

using System.Data.SqlClient;
using System.Data;
using Foreworld.Db;

using System.Data.OleDb;

using Foreworld.Cmd.Blog.Service;
using Foreworld.Cmd.Blog.Service.Impl;

namespace Foreworld.Cmd.Blog.Test
{
    using Category = Foreworld.Cmd.Blog.Model.Category;


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
            Console.WriteLine("Hello, World!!!");

            CategoryService service = new CategoryServiceImpl();
            List<Category> list = service.GetCategorys();

            for (int i = 0, j = list.Count; i < j; i++)
            {
                Category ca = list[i];
                Console.WriteLine(ca.CategoryName);
            }

            Console.WriteLine(list.Count);

            string sss = "2009-06-26 10:01:53";
            Console.WriteLine(Convert.ToDateTime(sss).Month);

            Console.WriteLine(DateTime.Now.ToString("yyyy年MM月dd日"));

            Console.WriteLine(Guid.NewGuid().ToString().Replace("-", ""));
        }
    }
}
