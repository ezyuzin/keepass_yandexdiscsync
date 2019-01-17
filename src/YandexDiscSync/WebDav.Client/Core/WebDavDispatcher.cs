using System;
using System.Net;
using System.Threading;

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

            httpRequest.ReadWriteTimeout = 5000;
            httpRequest.Timeout = 5000;

            foreach(var httpheader in Headers)
            {
                httpRequest.Headers.Add(httpheader.Name, httpheader.Value);
            }

            foreach (var httpheader in requestParams.Headers)
            {
                httpRequest.Headers.Add(httpheader.Name, httpheader.Value);
            }

           if (requestParams.Content != null)
           {
               if (!string.IsNullOrEmpty(requestParams.ContentType))
                   httpRequest.ContentType = requestParams.ContentType;

               var data = requestParams.Content.GetContent();
               httpRequest.ContentLength = data.Length;

               var requestStream = httpRequest.GetRequestStream();
               var buf = new byte[512];
               long count = 0;
               while (true)
               {
                   var nRead = data.Read(buf, 0, buf.Length);
                   if (nRead == 0)
                       break;

                   count += nRead;
                   requestStream.Write(buf, 0, nRead);
               }

           }

            try
            {
                return (HttpWebResponse) httpRequest.GetResponse();
            }
            catch (WebException webException)
            {
                return (HttpWebResponse) webException.Response;
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
