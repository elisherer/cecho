using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using il.co.sherer.cecho;
using System.Diagnostics;

namespace cecho.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Help()
        {
            Debug.WriteLine(DateTime.Now + ": Test Started [Help]");

            using (StringWriter sw = new StringWriter())
            {
                
                Console.SetOut(sw);

                Program.Main(new string[0]);

                var output = sw.ToString();
                Assert.IsTrue(output.StartsWith("cecho"));
            }

            Debug.WriteLine(DateTime.Now + ": Test Ended [Help]");
        }
    }
}
