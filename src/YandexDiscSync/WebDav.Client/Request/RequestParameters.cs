using System.Collections.Generic;
using System.Collections.Specialized;

namespace WebDav
{
    internal class RequestParameters
    {
        public RequestParameters()
        {
            Headers = new List<HttpHeader>();
        }

        public List<HttpHeader> Headers { get; set; }

        public HttpContent Content { get; set; }
        public string ContentType { get; set; }
    }
}
