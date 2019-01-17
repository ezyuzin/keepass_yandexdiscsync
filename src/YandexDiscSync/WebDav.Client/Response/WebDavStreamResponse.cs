using System;
using System.IO;
using System.Net;

namespace WebDav
{
    /// <summary>
    /// Represents a response of the GET operation.
    /// The class has to be properly disposed.
    /// </summary>
    public class WebDavStreamResponse : WebDavResponse, IDisposable
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="WebDavStreamResponse"/> class.
        /// </summary>
        /// <param name="response">The raw http response.</param>
        public WebDavStreamResponse(HttpWebResponse response)
            : base(response)
        {
            var stream = new MemoryStream();
            Stream = stream;
            using (var content = response.GetResponseStream())
            {
                if (content != null)
                {
                    var nCount = response.ContentLength;

                    byte[] buf = new byte[512];
                    while (true)
                    {
                        int nBytes = content.Read(buf, 0, buf.Length);
                        stream.Write(buf, 0, nBytes);

                        nCount -= nBytes;
                        if (nCount == 0)
                            break;
                    }

                    stream.Position = 0;
                }
            }

        }

        /// <summary>
        /// Gets the stream of content of the resource.
        /// </summary>
        public Stream Stream { get; }

        public override string ToString()
        {
            return $"WebDAV stream response - StatusCode: {StatusCode}, Description: {Description}";
        }

        public void Dispose()
        {
            Stream?.Dispose();
        }
    }
}
