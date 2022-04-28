namespace WebDav
{
    internal class HttpMethod
    {
        public static readonly HttpMethod Put = new HttpMethod("PUT");
        public static readonly HttpMethod Get = new HttpMethod("GET");
        public static readonly HttpMethod Post = new HttpMethod("POST");
        public static readonly HttpMethod Delete = new HttpMethod("DELETE");


        public string Method { get; }

        public HttpMethod(string method)
        {
            Method = method;
        }
    }
}