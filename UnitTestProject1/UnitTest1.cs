using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Models;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            JobLogger tp = new JobLogger(true, false, false, true, true, false);
            Assert.Fail();
        }
    }
}
