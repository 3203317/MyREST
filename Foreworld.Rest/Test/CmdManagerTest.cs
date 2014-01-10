using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Foreworld.Cmd;

namespace Foreworld.Rest.Test
{
    [TestFixture]
    public class CmdManagerTest
    {

        [Test]
        public void test()
        {
            Assert.AreEqual("121", "121");
            System.Diagnostics.Debug.WriteLine("----------------------------");

            //CmdManager _cm = new CmdManager(@"D:\dotnet\net.foreworld.rest\Foreworld.Cmd.Sysmanage\bin");

            //ICmd __cmd = _cm.GetCmd("LoginCmd");

            //System.Diagnostics.Debug.WriteLine(__cmd.Execute());

            //System.Diagnostics.Debug.WriteLine("----------------------------");
        }
    }
}
