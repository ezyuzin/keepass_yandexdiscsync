﻿using System.Collections.Generic;
using System.Threading;

namespace WebDav
{
    /// <summary>
    /// Represents parameters for the GET WebDAV method.
    /// </summary>
    public class GetFileParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFileParameters"/> class.
        /// </summary>
        public GetFileParameters()
        {
            Headers = new HttpHeaderCollection();
        }

        /// <summary>
        /// Gets or sets the collection of http request headers.
        /// </summary>
        public HttpHeaderCollection Headers { get; set; }
    }
}