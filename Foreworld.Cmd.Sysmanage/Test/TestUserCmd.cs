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

//using Foreworld.Log;
//using Foreworld.Cmd.Privilege.Model;

//namespace Foreworld.Cmd.Sysmanage.Test
//{
//    [TestFixture]
//    class TestUserCmd
//    {
//        private static readonly ILog _log = LogManager.GetLogger(typeof(TestUserCmd));

//        [SetUp]
//        public void Setup()
//        {
//            log4net.Config.XmlConfigurator.Configure();
//            MockHttpContext.Init();

//            string __session = "{'id':'1','sex':'男'}";
//            Object __obj = JavaScriptConvert.DeserializeObject(__session);
//            HttpContext.Current.Items["userInfo"] = __obj;
//        }

//        #region 测试CRUD

//        private string testAdd()
//        {

//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];


//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("username", "test用户");
//            __params.Add("password", "123456");
//            __params.Add("sex", "1");
//            __params.Add("alias", "屌丝男");
//            __params.Add("firstname", "煞");
//            __params.Add("lastname", "笔");
//            __params.Add("qq", "10001");

//            __params.Add("email", "diaosi@qq.com");
//            __params.Add("idcard", "410101010101010101");
//            __params.Add("mobile", "13837100001");
//            __params.Add("fax", "037166688822");

//            AddUserCmd __cmd = new AddUserCmd();

//            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params)).ToString();

//            JavaScriptObject __jso = (JavaScriptObject)JavaScriptConvert.DeserializeObject(__result);
//            JavaScriptArray __jsa = (JavaScriptArray)__jso["items"];
//            JavaScriptObject __obj = (JavaScriptObject)__jsa[0];


//            return __obj["id"].ToString();
//        }

//        private void testEdit(string @id)
//        {
//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];


//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("id", @id);

//            __params.Add("password", "123456");
//            __params.Add("sex", "2");
//            __params.Add("alias", "屌丝女");
//            __params.Add("firstname", "煞");
//            __params.Add("lastname", "笔");
//            __params.Add("qq", "10001");

//            __params.Add("email", "diaosi@qq.com");
//            __params.Add("idcard", "410101010101010101");
//            __params.Add("mobile", "13837100001");
//            __params.Add("fax", "037166688822");

//            UpdateUserCmd __cmd = new UpdateUserCmd();

//            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params)).ToString();
//        }

//        private void testDel(string @id)
//        {
//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];

//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("ids", @id);

//            DeleteUserCmd __cmd = new DeleteUserCmd();

//            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params)).ToString();
//        }

//        [Test]
//        public void test_CRUD()
//        {
//            TestUserCmd __t = new TestUserCmd();
//            string __id = __t.testAdd();

//            __t.test_List();

//            __t.testEdit(__id);

//            __t.test_List();

//            __t.testDel(__id);

//            __t.test_List();
//        }

//        #endregion

//        #region 测试查询

//        [Test]
//        public void test_List()
//        {

//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];

//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            //__params.Add("realname", "黄");
//            __params.Add("currentpage", "1");
//            __params.Add("pagesize", "200");
//            //__params.Add("username", "w");


//            ListUsersCmd __cmd = new ListUsersCmd();

//            string __result = __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params)).ToString();
//        }

//        #endregion

//        #region 测试用户登陆

//        [Test]
//        public void test_Login()
//        {

//            //Assert.AreEqual("121", "121");


//            //string username = "h1111111111111111";
//            //string password = "888222hx";

//            //string regex = "^[a-zA-Z][a-zA-Z0-9_-]{3,15}$";

//            //Console.WriteLine(regex);
//            //Console.WriteLine(Regex.IsMatch(username, regex));


//            LoginCmd __cmd = new LoginCmd();
//            //System.Diagnostics.Debug.WriteLine("--" + __cmd.ConnectionString);

//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("username", "admin");
//            __params.Add("password", "123456");

//            __cmd.Execute(new Parameter(Response.JSON, null, __params));


//        }
//        #endregion

//        #region 测试用户退出

//        [Test]
//        public void test_Logout()
//        {
//            _log.Debug("用户退出功能测试");
//            LogoutCmd __cmd = new LogoutCmd();

//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
//            LogInfo __logInfo = new LogInfo();

//            __cmd.Execute(new Parameter(Response.JSON, __userInfo, __logInfo, null));
//        }
//        #endregion

//        #region 测试用户菜单树列表

//        [Test]
//        public void test_ListUserMenuTree()
//        {
//            ListUserMenuTreeCmd __cmd = new ListUserMenuTreeCmd();
//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
//            LogInfo __logInfo = new LogInfo();

//            Parameter __parameter = new Parameter(Response.JSON, __userInfo, __logInfo, null);
//            __cmd.Execute(__parameter);
//        }
//        #endregion

//        #region 测试用户常用菜单树列表

//        [Test]
//        public void test_ListUserAllMenuTree()
//        {
//            ListUserAllMenuTreeCmd __cmd = new ListUserAllMenuTreeCmd();
//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];
//            LogInfo __logInfo = new LogInfo();

//            Parameter __parameter = new Parameter(Response.JSON, __userInfo, __logInfo, null);
//            __cmd.Execute(__parameter);
//        }
//        #endregion

//        #region 重置我的密码

//        [Test]
//        public void test_ResetMyPassword()
//        {
//            _log.Debug("重置我的密码功能测试");


//            ResetMyPasswordCmd __cmd = new ResetMyPasswordCmd();

//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("password", "123456");
//            __params.Add("opassword", "1234567");
//            //__params.Add("opassword", "123456");

//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];

//            __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params));

//        }
//        #endregion

//        #region 重置用户密码


//        [Test]
//        public void test_ResetUserPassword()
//        {
//            _log.Debug("重置用户密码功能测试");


//            ResetUserPasswordCmd __cmd = new ResetUserPasswordCmd();

//            Dictionary<string, string> __params = new Dictionary<string, string>();

//            __params.Add("userid", "2");
//            __params.Add("password", "1234567");
//            //__params.Add("opassword", "123456");

//            JavaScriptObject __userInfo = (JavaScriptObject)HttpContext.Current.Items["userInfo"];

//            __cmd.Execute(new Parameter(Response.JSON, __userInfo, __params));


//        }
//        #endregion

//        #region 测试验证用户Key

//        [Test]
//        public void test_VerifyUserKeys()
//        {
//            VerifyUserKeysCmd __cmd = new VerifyUserKeysCmd();

//            Dictionary<string, string> __params = new Dictionary<string, string>();
//            __params.Add("apikey", "123456");

//            LogInfo __logInfo = new LogInfo();

//            __cmd.Execute(new Parameter(Response.JSON, null, __logInfo, __params));

//        }
//        #endregion

//        #region 测试Rest服务接口密钥功能

//        [Test]
//        public void test_ApiKey()
//        {
//            ArrayList __a = new ArrayList();

//            __a.Add("abd=1111");
//            __a.Add("ab1cd=2222");
//            __a.Add("ab1cd=3333");
//            __a.Add("cb=4444");

//            __a.Sort();


//            string[] __b = (string[])__a.ToArray(typeof(string));




//            for (int __i_3 = 0, __j_3 = __a.Count; __i_3 < __j_3; __i_3++)
//            {
//                System.Diagnostics.Debug.WriteLine(__a[__i_3]);
//            }




//            System.Diagnostics.Debug.WriteLine(__a.ToString());

//            System.Diagnostics.Debug.WriteLine(string.Join("&", __b));


//            System.Diagnostics.Debug.WriteLine("--------------------");

//            Dictionary<string, string> __t = new Dictionary<string, string>();


//            __t.Add("username", "admin".Substring(0, 4));
//            __t.Add("opassword", "654321");
//            __t.Add("password", "123456");
//            __t.Add("t", "1234567890");

//            if (!__t.ContainsKey("opassword"))
//                __t.Add("opassword", "123456");


//            System.Diagnostics.Debug.WriteLine("Count: " + __t.Count);

//            ArrayList __c = new ArrayList();

//            foreach (string __key_3 in __t.Keys)
//            {
//                System.Diagnostics.Debug.WriteLine(__key_3 + ": " + __t[__key_3]);
//                __c.Add(__key_3 + "=" + __t[__key_3]);
//            }

//            __c.Sort();

//            string[] __c1 = (string[])__c.ToArray(typeof(string));
//            System.Diagnostics.Debug.WriteLine(string.Join("&", __c1));





//            System.Diagnostics.Debug.WriteLine("--------------------Base64");

//            string __data = "apiKey=J4_EFO3ZlBZynJC7dACIFiivoCniAvJlLr-H_dIex-eAdyz1ykGgMtrvcJ7PBCrPKsJRuPaiRKdDuL5LTL_Jag&command=listZones&response=json";
//            string __key = "KFD85H9SmyZd8FSopX_CxxG5VgLFW71LiYc35PxZWXABX9BsANvPUQpLBCrPz25JpSy2_bt2Z0gWRCA6ePsKww";


//            HMACSHA1 hmacSha = new HMACSHA1(Encoding.Default.GetBytes(__key));
//            hmacSha.Initialize();
//            byte[] hmac = hmacSha.ComputeHash(Encoding.Default.GetBytes(__data.ToLower()));





//            //  System.Diagnostics.Debug.WriteLine(HMACSHA1Encrypt(__data.ToLower(), __key));

//            //byte[] bytes = Encoding.Default.GetBytes("sdfdsf$sdf23!&^(*&@#@!要转换的字符串");
//            string __base64 = Convert.ToBase64String(hmac);

//            System.Diagnostics.Debug.WriteLine(__base64);

//            string __encode = HttpUtility.UrlEncode(__base64, Encoding.UTF8);



//            System.Diagnostics.Debug.WriteLine(__encode);

//            string post = SendDataByPost("http://localhost:50048/Foreworld.Web/Api.ashx", "a=b&c=d");


//            System.Diagnostics.Debug.WriteLine(post);
//        }

//        public static string HMACSHA1Encrypt(string EncryptText, string EncryptKey)
//        {
//            byte[] StrRes = Encoding.Default.GetBytes(EncryptText);
//            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.Default.GetBytes(EncryptKey));

//            CryptoStream CStream = new CryptoStream(Stream.Null, myHMACSHA1, CryptoStreamMode.Write);
//            CStream.Write(StrRes, 0, StrRes.Length);
//            StringBuilder EnText = new StringBuilder();
//            foreach (byte Byte in StrRes)
//            {
//                EnText.AppendFormat("{0:x2}", Byte);
//            }
//            return EnText.ToString();
//        }

//        public string SendDataByPost(string Url, string postDataStr)
//        {
//            ServicePointManager.Expect100Continue = false;
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
//            request.Method = "POST";
//            request.Timeout = 5000;
//            request.ContentType = "application/x-www-form-urlencoded";
//            request.ContentLength = postDataStr.Length;
//            Stream myRequestStream = request.GetRequestStream();
//            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
//            myStreamWriter.Write(postDataStr);
//            myStreamWriter.Close();
//            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//            Stream myResponseStream = response.GetResponseStream();
//            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
//            string retString = myStreamReader.ReadToEnd();
//            myStreamReader.Close();
//            myResponseStream.Close();
//            return retString;
//        }
//        #endregion

//        [Test]
//        public void test_test()
//        {
//            //UserDao __userDao = new UserDaoImpl();
//            //Foreworld.Cmd.Sysmanage.User.User __user = new Foreworld.Cmd.Sysmanage.User.User();
//            //__user.UserName = "hx";
//            //__user.IsEnable = 1;
//            //__user.IsInvalid = 1;
//            //__user = __userDao.query(__user);
//            //System.Diagnostics.Debug.WriteLine(__user == null ? "空" : __user.UserName);
//        }
//    }
//}