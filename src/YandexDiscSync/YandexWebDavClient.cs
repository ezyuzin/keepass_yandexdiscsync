using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using KeePassLib.Security;
using WebDav;

namespace YandexDiscSync
{
    internal class YandexDiscFile
    {
        public string Location { get; }
        public Uri Uri { get; }

        public YandexDiscFile(string filename, string location, WebDavClient webDav)
        {
            filename = filename.Replace("\\", "/").Trim('/') + ".db3";
            Location = Path.Combine(location, filename)
                .Replace("\\", "/")
                .Trim('/');

            Uri = new Uri($"{webDav.BaseAddress}{Location}");
        }
    }

    internal class YandexWebDavClient
    {
        public delegate void ProgressCallback(double progress);
        public string Username { get;set; }
        public ProtectedString Password { get;set; }

        public string Location { get;set; }


        public YandexWebDavClient()
        {
        }

        public YandexWebDavClient(YandexDiscSyncConf syncConf)
        {
            Username = syncConf.Username;
            Password = syncConf.Password;
            Location = syncConf.Location;
        }

        public void PutFile(string filename1, byte[] bytes, DateTime lastModified, ProgressCallback progressCallback)
        {
            var webDav = GetWebDavClient();
            var yaFile = new YandexDiscFile(filename1, Location, webDav);

            CreateDirectory(Path.GetDirectoryName(yaFile.Location), webDav);

            using (var stream = new MemoryStream(bytes))
            {
                var props = new PutFileParameters();
                props.OperationProgress = args =>
                {
                  progressCallback?.Invoke(args.Progress);
                };

                var response = webDav.PutFile(yaFile.Uri, stream, props);
                if (!response.IsSuccessful)
                    throw new ApplicationException($"Unable save file: --> {response.StatusCode} {response.Description}");
            }

            RetryIfFail(5, () =>
            {
                var name = XName.Get("x-lastmodified", "DataSync");

                var patch = new ProppatchParameters();
                patch.PropertiesToSet.Add(name, lastModified.ToString("u"));
                patch.Namespaces.Add(new NamespaceAttr("u", "DataSync"));

                var propPatch = webDav.Proppatch(yaFile.Uri, patch);
                if (!propPatch.IsSuccessful)
                    throw new ApplicationException($"Unable update file properties: --> {propPatch.StatusCode} {propPatch.Description}");

                return true;
            });
        }

        private void CreateDirectory(string filepath, WebDavClient webDav)
        {
            var pathItems = filepath.Replace("\\", "/").Split('/');
            var path = "";
            for (int i = 0; i < pathItems.Length; i++)
            {
                path = path + pathItems[i];
                var propParam = new PropfindParameters();
                RetryIfFail(5, () =>
                {
                    var propFind = webDav.PropFind(new Uri($"{webDav.BaseAddress}{path}"), propParam);
                    if (!propFind.IsSuccessful)
                    {
                        if (propFind.StatusCode != 404)
                            throw new ApplicationException($"Unable read file props: --> {propFind.StatusCode} {propFind.Description}");

                        var folder = webDav.Mkcol(new Uri($"{webDav.BaseAddress}{path}"));
                        if (!folder.IsSuccessful)
                            throw new ApplicationException($"Unable create folder: --> {folder.StatusCode} {folder.Description}");
                    }
                    return propFind;
                });
                path = path + "/";
            }
        }

        public byte[] GetFile(string filename1, ProgressCallback progressCallback)
        {
            var webDav = GetWebDavClient();
            var yaFile = new YandexDiscFile(filename1, Location, webDav);

            var props = new GetFileParameters();
            props.OperationProgress = args =>
            {
              progressCallback?.Invoke(args.Progress);
            };

            using (var response = webDav.GetFile(yaFile.Uri, false, props))
            {
                if (!response.IsSuccessful)
                    throw new ApplicationException($"Unable read file: --> {response.StatusCode} {response.Description}");

                var data = new byte[response.Stream.Length];
                response.Stream.Read(data, 0, data.Length);
                return data;
            }
        }

        private WebDavClient GetWebDavClient()
        {
            WebDavClient webDav = new WebDavClient(new Uri("https://webdav.yandex.ru"));
            webDav.HttpHeaders["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password.ReadString()}"));
            return webDav;
        }

        public bool IsFileExist(string filename1)
        {
            var webDav = GetWebDavClient();
            var yaFile = new YandexDiscFile(filename1, Location, webDav);

            var propParam = new PropfindParameters();
            RetryIfFail(5, () =>
            {
                var propFind1 = webDav.PropFind(yaFile.Uri, propParam);
                if (!propFind1.IsSuccessful)
                {
                    if (propFind1.StatusCode == 404)
                        return false;

                    throw new ApplicationException($"Unable read file props: --> {propFind1.StatusCode} {propFind1.Description}");
                }
                return true;
            });
            return true;
        }

        public DateTime GetLastModified(string filename1)
        {
            var webDav = GetWebDavClient();
            var yaFile = new YandexDiscFile(filename1, Location, webDav);

            var propParams = new PropfindParameters();
            propParams.Namespaces.Add(new NamespaceAttr("u", "DataSync"));
            propParams.CustomProperties.Add(XName.Get("x-lastmodified", "DataSync"));

            var propFind = RetryIfFail(5, () =>
            {
                var propFind1 = webDav.PropFind(yaFile.Uri, propParams);
                if (!propFind1.IsSuccessful)
                {
                    throw new ApplicationException($"Unable read file props: --> {propFind1.StatusCode} {propFind1.Description}");
                }
                return propFind1;
            });

            var resource = propFind
                .Resources
                .FirstOrDefault(m => m.Uri == $"/{yaFile.Location}");

            var prop = resource?.Properties.FirstOrDefault(m => m.Name.NamespaceName == "DataSync" && m.Name.LocalName == "x-lastmodified");
            if (prop != null && !string.IsNullOrEmpty(prop.Value))
            {
                return DateTime.Parse(prop.Value).ToUniversalTime();
            }

            return DateTime.MinValue;
        }

        public static T RetryIfFail<T>(int count, Func<T> method)
        {
            while (true)
            {
                try
                {
                    return method.Invoke();
                }
                catch
                {
                    if (--count < 0)
                        throw;

                    Thread.Sleep(500);
                }
            }
        }
    }
}