using System;
using System.IO;

namespace WebDav
{
    public abstract class HttpContent : IDisposable
    {
        public abstract Stream GetContent();

        public void Dispose()
        {
            Dispose(true);
        }

        protected abstract void Dispose(bool disposing);
    }
}