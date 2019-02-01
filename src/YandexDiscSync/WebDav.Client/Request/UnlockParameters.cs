namespace WebDav
{
    /// <summary>
    /// Represents parameters for the UNLOCK WebDAV method.
    /// </summary>
    public class UnlockParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnlockParameters"/> class.
        /// <param name="lockToken">The resource lock token.</param>
        /// </summary>
        public UnlockParameters(string lockToken)
        {
            Guard.NotNull(lockToken, "lockToken");

            LockToken = lockToken;
            Headers = new HttpHeaderCollection();
        }

        /// <summary>
        /// Gets the resource lock token.
        /// </summary>
        public string LockToken { get; }

        /// <summary>
        /// Gets or sets the collection of http request headers.
        /// </summary>
        public HttpHeaderCollection Headers { get; set; }
    }
}
