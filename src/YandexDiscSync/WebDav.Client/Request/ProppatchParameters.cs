using System.Collections.Generic;
using System.Xml.Linq;

namespace WebDav
{
    /// <summary>
    /// Represents parameters for the PROPPATCH WebDAV method.
    /// </summary>
    public class ProppatchParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProppatchParameters"/> class.
        /// </summary>
        public ProppatchParameters()
        {
            PropertiesToSet = new Dictionary<XName, string>();
            PropertiesToRemove = new List<XName>();
            Namespaces = new List<NamespaceAttr>();
            Headers = new HttpHeaderCollection();
        }

        /// <summary>
        /// Gets or sets properties to set on the resource.
        /// </summary>
        public IDictionary<XName, string> PropertiesToSet { get; set; }

        /// <summary>
        /// Gets or sets the collection of properties defined on the resource to remove.
        /// </summary>
        public List<XName> PropertiesToRemove { get; set; }

        /// <summary>
        /// Gets or sets the collection of xml namespaces of properties.
        /// </summary>
        public List<NamespaceAttr> Namespaces { get; set; }

        /// <summary>
        /// Gets or sets the collection of http request headers.
        /// </summary>
        public HttpHeaderCollection Headers { get; set; }

        /// <summary>
        /// Gets or sets the resource lock token.
        /// </summary>
        public string LockToken { get; set; }
    }
}
