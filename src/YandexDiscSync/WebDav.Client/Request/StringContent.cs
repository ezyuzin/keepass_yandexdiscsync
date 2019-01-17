using System.IO;
using System.Text;

namespace WebDav
{
    internal class StreamContent : HttpContent
    {
        private Stream _stream;
        public StreamContent(Stream content)
        {
            _stream = content;
        }

        public override Stream GetContent()
        {
            return _stream;
        }

        protected override void Dispose(bool disposing)
        {
        }
    }

    internal class StringContent : HttpContent
    {
        private MemoryStream _content;
        public StringContent(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            _content = new MemoryStream(bytes);
        }

        public override Stream GetContent()
        {
            return _content;
        }

        protected override void Dispose(bool disposing)
        {
            _content?.Dispose();
            _content = null;
        }
    }
}