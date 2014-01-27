using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.Reflection;
using System.Diagnostics;

using NUnit.Framework;
using Newtonsoft.Json;
using Foreworld.Cmd.Privilege.Model;
using Foreworld.Cmd.Sysmanage.Model;

namespace Foreworld.Cmd.Sysmanage.Test
{
    [TestFixture]
    class TestRegex
    {
        [Test]
        public void test()
        {
            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("1111111111111111111", "^\\d+$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("1111,21,4334", "^(\\d+,?)+$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("121293,149,5876", "^(\\d+,?)*\\d+$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊", "^.{1,20}$"));



            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("1", "^[A-Z1-9][A-Z0-9]{0,19}$"));


            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("1,AZ", "^[A-Z1-9][A-Z0-9]{0,19},[A-Z]{2,20}$"));


            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("阿啊哦啊啊啊啊啊1分", "^[\u4E00-\u9FA5a-zA-Z0-9_]{2,10}$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("100", "^(20|50|100|200)$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("10", "^[^0]\\d{1,9}$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("", "^.{0,20}$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("11,2收到1,31ss", "^([\u4E00-\u9FA5a-zA-Z0-9_]{2,10},?)*[\u4E00-\u9FA5a-zA-Z0-9_]{2,10}$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("11,所得税", "^([\u4E00-\u9FA5a-zA-Z0-9_]{2,10},?)*[\u4E00-\u9FA5a-zA-Z0-9_]{2,10}$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("所得税", "^([\u4E00-\u9FA5a-zA-Z0-9_]{2,10},?)*[\u4E00-\u9FA5a-zA-Z0-9_]{2,10}$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("FF,AA", "^([A-Z_]{2,20},?)*[A-Z_]{2,20}$"));

            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("dhtmlx", "^(dojo|dhtmlx)$"));



            //System.Net.WebClient web = new System.Net.WebClient();

            //byte[] buffer = web.DownloadData("http://localhost/Remote.ashx?command=login&apikey=1&signature=1&time=1&username=admin&password=123456");
            //System.Diagnostics.Debug.WriteLine(System.Text.Encoding.GetEncoding("UTF-8").GetString(buffer));



            //System.Net.WebRequest web = System.Net.WebRequest.Create("http://localhost/Remote.ashx?command=login&apikey=1&signature=1&time=1&username=admin&password=123456");

            //System.IO.Stream rc = web.GetResponse().GetResponseStream();
            //System.IO.StreamReader read = new System.IO.StreamReader(rc, System.Text.Encoding.GetEncoding("UTF-8"));
            //System.Diagnostics.Debug.WriteLine(read.ReadToEnd());



            DateTime timeStamp = new DateTime(1970, 1, 1); //得到1970年的时间戳 
            long a = (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000; //注意这里有时区问题，用now就要减掉8个小时
            System.Diagnostics.Debug.WriteLine(a.ToString());


            long unixTime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            System.Diagnostics.Debug.WriteLine(unixTime.ToString());


            /* WHERE参数的正则表达式 */
            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("AND,C_2,>,|XOR,B1B,<,'|NOT AND,HAHA,<=,'", "^((AND|OR|XOR|NOT AND)\\,[0-9A-Z_]+\\,(>|<|=|>=|<=|<>)\\,'?\\|?)*$"));

            /* 防SQL注入过滤非法字段 */
            String reg = "(select|update|insert|delete|declare|@|exec|dbcc|alter|drop|create|backup|if|else|end|set|open|close|use|begin|retun|as|go|exists)+";
            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("s haha and or ' ", reg));

            /* 不匹配的字符串 */
            reg = "^(?!.*(select|update)).*$";
            System.Diagnostics.Debug.WriteLine(Regex.IsMatch(" haha elupdat   and oselect r ' ", reg));

            reg = "^\\d{0,3}$";
            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("000", reg));

            /* 32位的大写字母和数字组成 */
            System.Diagnostics.Debug.WriteLine(Regex.IsMatch("1a028b881414eca7601414ecd6f480001", "^[0-9a-z]{32}$"));

            string url = "/article/abc/4028b881414eca7601414ecd6f480001.html";
            System.Diagnostics.Debug.WriteLine(url.LastIndexOf('/'));
            System.Diagnostics.Debug.WriteLine(url.Substring(0, url.LastIndexOf('/')));

            string __rest = "/user/login";
            string[] __rests = __rest.Split('/');
            System.Diagnostics.Debug.WriteLine(__rests[1]);

            string __rest2 = "/sysmanage/user/login";
            string __rest2_1 = __rest2.Substring(0, __rest2.LastIndexOf('/'));
            System.Diagnostics.Debug.WriteLine(__rest2_1);


            string __json = "{'UserName':'hx','UserPass':'123456','VerifyCode':'haha'}";
            LoginForm __jsObj = JavaScriptConvert.DeserializeObject<LoginForm>(__json);
            System.Diagnostics.Debug.WriteLine(__jsObj.UserName);

            System.Diagnostics.Debug.WriteLine(this.GetType().Assembly.GetName().Name);


            StackFrame __sf = new StackFrame(1);
            string __tplName = __sf.GetMethod().Name;
            string __clsName = __sf.GetMethod().ReflectedType.Name;
            System.Diagnostics.Debug.WriteLine(__clsName);


            System.Diagnostics.Debug.WriteLine("----------");
            string abc = "#parse('pagelet/TopSearchForm.html');";
            Regex rege = new Regex("#parse\\(\\'(.*)\\'\\);");
            MatchCollection mmc = rege.Matches(abc);
            Console.WriteLine(mmc[0].Groups[1].Value);
        }
    }
}
