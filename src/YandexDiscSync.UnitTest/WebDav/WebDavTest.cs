using System;
using System.IO;
using System.Reflection;
using KeePassLib.Security;
using NUnit.Framework;

namespace YandexDiscSync.UnitTest.WebDav
{
    [TestFixture]
    public class WebDavTest
    {
        [SetUp]
        public void SetUp()
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.CurrentDirectory = location;
        }

        [Test]
        public void FileGetTest()
        {
            YandexWebDavClient client = new YandexWebDavClient();
            client.Username = "mytestaccount@yandex.ru";
            client.Password = new ProtectedString(true, "1234");

            var lastModified = client.GetLastModified("js1.zip");
            Console.WriteLine(lastModified);

            var bytes = client.GetFile("js1.zip", (msg) => { Console.WriteLine(msg); });
            Console.WriteLine(bytes.Length);

            var content = File.ReadAllBytes(@"sign.snk");
            client.PutFile("abcd/abcd/js1.zip", content, DateTime.UtcNow, (msg) => { Console.WriteLine(msg); });


        }
    }
}