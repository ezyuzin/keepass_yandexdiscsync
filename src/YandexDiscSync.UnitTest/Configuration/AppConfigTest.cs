using System;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using SafeVault.Configuration;
using SafeVault.Security;

namespace SafeVault.Core.UnitTest
{
    [TestFixture]
    public class AppConfigTest
    {
        [SetUp]
        public void SetUp()
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.CurrentDirectory = location;
        }

        [Test]
        public void AppConfig020Test()
        {
            try
            {
                var appConf1 = new AppConfig("data/test.config");

                Assert.AreEqual("INFO", appConf1.Get("logger/root/level"));
                appConf1.Set("logger/root/level", "DEBUG");
                appConf1.Save("data/test1.config");

                var appConf2 = new AppConfig("data/test.config");
                Assert.AreEqual("INFO", appConf2.Get("logger/root/level"));

                var appConf3 = new AppConfig("data/test1.config");
                Assert.AreEqual("DEBUG", appConf3.Get("logger/root/level"));
            }
            finally
            {
                if (File.Exists("data/test1.config"))
                    File.Delete("data/test1.config");
            }
        }

        [Test]
        public void AppConfig010Test()
        {
            var appConf = new AppConfig("data/test.config");

            Assert.AreEqual("INFO", appConf.Get("logger/root/level"));
            Assert.AreEqual("rootNode", appConf.Get("logger/root/@name"));

            var section = appConf.GetSection("logger");
            Assert.AreEqual("INFO", section.Get("root/level"));
            Assert.AreEqual("rootNode", section.Get("root/@name"));

            Assert.AreEqual(2, section.GetSections("appenders/appender").Length);

        }
    }
}