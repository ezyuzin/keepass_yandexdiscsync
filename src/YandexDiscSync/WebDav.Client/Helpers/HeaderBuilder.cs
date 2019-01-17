using System;
using System.Collections.Generic;
using System.Linq;

namespace WebDav
{
    public class HttpHeaderCollection : List<HttpHeader>
    {
        public string this[string name]
        {
            get { return this.FirstOrDefault(m => m.Name == name)?.Value; }
            set
            {
                var find = this.FirstOrDefault(m => m.Name == name);
                if (find == null)
                    this.Add(new HttpHeader(name, value));
                else
                    find.Value = value;
            }
        }
    }

    public class HttpHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public HttpHeader()
        {
        }

        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    internal class HeaderBuilder
    {
        private readonly List<HttpHeader> _headers;

        public HeaderBuilder()
        {
            _headers = new List<HttpHeader>();
        }

        public HeaderBuilder Add(string name, string value)
        {
            _headers.Add(new HttpHeader(name, value));
            return this;
        }

        public HeaderBuilder AddWithOverwrite(HttpHeaderCollection headers)
        {
            var headers1 = headers.ToArray();

            foreach (var header in headers1)
            {
                _headers.RemoveAll(x => x.Name.Equals(header.Name, StringComparison.CurrentCultureIgnoreCase));
            }
            _headers.AddRange(headers1);
            return this;
        }

        public List<HttpHeader> Build()
        {
            return _headers.ToList();
        }
    }
}
