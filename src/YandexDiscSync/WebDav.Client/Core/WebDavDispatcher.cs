using System;
using System.Net;

namespace WebDav
{
    internal interface IWebDavDispatcher
    {
        Uri BaseAddress { get; set; }
        HttpHeaderCollection Headers { get; }


        HttpWebResponse Send(
            Uri requestUri,
            HttpMethod httpMethod,
            RequestParameters requestParams);
    }

    internal class WebDavDispatcher : IWebDavDispatcher, IDisposable
    {
        public Uri BaseAddress { get; set; }
        public HttpHeaderCollection Headers { get; set; }

        public WebDavDispatcher()
        {
            Headers = new HttpHeaderCollection();
        }

        public HttpWebResponse Send(
            Uri requestUri,
            HttpMethod httpMethod,
            RequestParameters requestParams)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            httpRequest.Method = httpMethod.Method;
            httpRequest.Proxy = WebRequest.GetSystemWebProxy();
            if (httpRequest.Proxy != null)
                httpRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;

            foreach (var httpheader in Headers)
            {
                httpRequest.Headers.Add(httpheader.Name, httpheader.Value);
            }

            foreach (var httpheader in requestParams.Headers)
            {
                httpRequest.Headers.Add(httpheader.Name, httpheader.Value);
            }

            requestParams.OperationProgress?.Invoke(new WebDavOperationInfo { Progress = 0 });
            httpRequest.Timeout = 15000;
            httpRequest.ReadWriteTimeout = 15000;
            httpRequest.ContentLength = 0;

            if (requestParams.Content != null)
            {
                var data = requestParams.Content.GetContent();

                double progress = 0;
                requestParams.OperationProgress?.Invoke(new WebDavOperationInfo { Progress = 0 });

                if (data.Length > 0)
                {
                    var bufLen = (data.Length < 1 * 1024 * 1024)
                        ? data.Length
                        : 1 * 1024 * 1024;

                    var buf = new byte[bufLen];
                    long count = data.Length;

                    httpRequest.ContentLength = data.Length;
                    if (data.Length > 1 * 1024 * 1024)
                    {
                        httpRequest.AllowWriteStreamBuffering = false;
                    }

                    httpRequest.Timeout = 1000 * 60 * 60;
                    httpRequest.ReadWriteTimeout = 1000 * 60 * 60;
                    if (!string.IsNullOrEmpty(requestParams.ContentType))
                        httpRequest.ContentType = requestParams.ContentType;

                    using (var requestStream = httpRequest.GetRequestStream())
                    {
                        while (count > 0)
                        {
                            int nRead = (count < buf.Length) ? (int) count : buf.Length;

                            nRead = data.Read(buf, 0, nRead);
                            if (nRead > 0)
                            {
                                count -= nRead;
                                requestStream.Write(buf, 0, nRead);

                                if (requestParams.OperationProgress != null)
                                {
                                    double progress1 = 100.0 * (data.Length - count) / data.Length;
                                    if (progress1 - progress >= 0.1)
                                    {
                                        requestParams.OperationProgress?.Invoke(new WebDavOperationInfo {Progress = progress});
                                        progress = progress1;
                                    }
                                }
                            }
                        }
                    }
                }

                requestParams.OperationProgress?.Invoke(new WebDavOperationInfo { Progress = 100 });
            }

            try
            {
                return (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException webException)
            {
                return (HttpWebResponse)webException.Response;
            }
            finally
            {
            }
        }

        #region IDisposable

        public void Dispose()
        {
            DisposeManagedResources();
        }

        protected virtual void DisposeManagedResources()
        {
        }

        #endregion
    }
}
