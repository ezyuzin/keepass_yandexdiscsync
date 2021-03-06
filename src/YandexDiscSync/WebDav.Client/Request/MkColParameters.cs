﻿namespace WebDav
{
    /// <summary>
    /// Represents parameters for the MKCOL WebDAV method.
    /// </summary>
    public class MkColParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MkColParameters"/> class.
        /// </summary>
        public MkColParameters()
        {
            Headers = new HttpHeaderCollection();
        }

        /// <summary>
        /// Gets or sets the resource lock token.
        /// </summary>
        public string LockToken { get; set; }

        /// <summary>
        /// Gets or sets the collection of http request headers.
        /// </summary>
        public HttpHeaderCollection Headers { get; set; }
    }
}
