﻿using System;
using System.IO;

namespace WebDav
{
    /// <summary>
    /// Represents a WebDAV client that can perform WebDAV operations.
    /// </summary>
    public interface IWebDavClient : IDisposable
    {
        /// <summary>
        /// Retrieves properties defined on the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <returns>An instance of <see cref="PropfindResponse" />.</returns>
        PropfindResponse Propfind(string requestUri);

        /// <summary>
        /// Retrieves properties defined on the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <returns>An instance of <see cref="PropfindResponse" />.</returns>
        PropfindResponse Propfind(Uri requestUri);

        /// <summary>
        /// Retrieves properties defined on the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="parameters">Parameters of the PROPFIND operation.</param>
        /// <returns>An instance of <see cref="PropfindResponse" />.</returns>
        PropfindResponse Propfind(string requestUri, PropfindParameters parameters);

        /// <summary>
        /// Retrieves properties defined on the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="parameters">Parameters of the PROPFIND operation.</param>
        /// <returns>An instance of <see cref="PropfindResponse" />.</returns>
        PropfindResponse Propfind(Uri requestUri, PropfindParameters parameters);

        /// <summary>
        /// Sets and/or removes properties defined on the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="parameters">Parameters of the PROPPATCH operation.</param>
        /// <returns>An instance of <see cref="ProppatchResponse" />.</returns>
        ProppatchResponse Proppatch(string requestUri, ProppatchParameters parameters);

        /// <summary>
        /// Sets and/or removes properties defined on the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="parameters">Parameters of the PROPPATCH operation.</param>
        /// <returns>An instance of <see cref="ProppatchResponse" />.</returns>
        ProppatchResponse Proppatch(Uri requestUri, ProppatchParameters parameters);

        /// <summary>
        /// Creates a new collection resource at the location specified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Mkcol(string requestUri);

        /// <summary>
        /// Creates a new collection resource at the location specified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Mkcol(Uri requestUri);

        /// <summary>
        /// Creates a new collection resource at the location specified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="parameters">Parameters of the MKCOL operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Mkcol(string requestUri, MkColParameters parameters);

        /// <summary>
        /// Creates a new collection resource at the location specified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="parameters">Parameters of the MKCOL operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Mkcol(Uri requestUri, MkColParameters parameters);

        /// <summary>
        /// Retrieves the file identified by the request URI telling the server to return it without processing.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <returns>An instance of <see cref="WebDavStreamResponse" />.</returns>
        WebDavStreamResponse GetRawFile(string requestUri);

        /// <summary>
        /// Retrieves the file identified by the request URI telling the server to return it without processing.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <returns>An instance of <see cref="WebDavStreamResponse" />.</returns>
        WebDavStreamResponse GetRawFile(Uri requestUri);

        /// <summary>
        /// Retrieves the file identified by the request URI telling the server to return it without processing.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="parameters">Parameters of the GET operation.</param>
        /// <returns>An instance of <see cref="WebDavStreamResponse" />.</returns>
        WebDavStreamResponse GetRawFile(string requestUri, GetFileParameters parameters);

        /// <summary>
        /// Retrieves the file identified by the request URI telling the server to return it without processing.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="parameters">Parameters of the GET operation.</param>
        /// <returns>An instance of <see cref="WebDavStreamResponse" />.</returns>
        WebDavStreamResponse GetRawFile(Uri requestUri, GetFileParameters parameters);

        /// <summary>
        /// Retrieves the file identified by the request URI telling the server to return a processed response, if possible.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <returns>An instance of <see cref="WebDavStreamResponse" />.</returns>
        WebDavStreamResponse GetProcessedFile(string requestUri);

        /// <summary>
        /// Retrieves the file identified by the request URI telling the server to return a processed response, if possible.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <returns>An instance of <see cref="WebDavStreamResponse" />.</returns>
        WebDavStreamResponse GetProcessedFile(Uri requestUri);

        /// <summary>
        /// Retrieves the file identified by the request URI telling the server to return a processed response, if possible.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="parameters">Parameters of the GET operation.</param>
        /// <returns>An instance of <see cref="WebDavStreamResponse" />.</returns>
        WebDavStreamResponse GetProcessedFile(string requestUri, GetFileParameters parameters);

        /// <summary>
        /// Retrieves the file identified by the request URI telling the server to return a processed response, if possible.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="parameters">Parameters of the GET operation.</param>
        /// <returns>An instance of <see cref="WebDavStreamResponse" />.</returns>
        WebDavStreamResponse GetProcessedFile(Uri requestUri, GetFileParameters parameters);

        /// <summary>
        /// Deletes the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Delete(string requestUri);

        /// <summary>
        /// Deletes the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Delete(Uri requestUri);

        /// <summary>
        /// Deletes the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="parameters">Parameters of the DELETE operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Delete(string requestUri, DeleteParameters parameters);

        /// <summary>
        /// Deletes the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="parameters">Parameters of the DELETE operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Delete(Uri requestUri, DeleteParameters parameters);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="stream">The stream of content of the resource.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(string requestUri, Stream stream);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="stream">The stream of content of the resource.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(Uri requestUri, Stream stream);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="stream">The stream of content of the resource.</param>
        /// <param name="contentType">The content type of the request body.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(string requestUri, Stream stream, string contentType);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="stream">The stream of content of the resource.</param>
        /// <param name="contentType">The content type of the request body.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(Uri requestUri, Stream stream, string contentType);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="stream">The stream of content of the resource.</param>
        /// <param name="parameters">Parameters of the PUT operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(string requestUri, Stream stream, PutFileParameters parameters);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="stream">The stream of content of the resource.</param>
        /// <param name="parameters">Parameters of the PUT operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(Uri requestUri, Stream stream, PutFileParameters parameters);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="content">The content to pass to the request.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(string requestUri, HttpContent content);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="content">The content to pass to the request.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(Uri requestUri, HttpContent content);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="content">The content to pass to the request.</param>
        /// <param name="parameters">Parameters of the PUT operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(string requestUri, HttpContent content, PutFileParameters parameters);

        /// <summary>
        /// Requests the resource to be stored under the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="content">The content to pass to the request.</param>
        /// <param name="parameters">Parameters of the PUT operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse PutFile(Uri requestUri, HttpContent content, PutFileParameters parameters);

        /// <summary>
        /// Creates a duplicate of the source resource identified by the source URI in the destination resource identified by the destination URI.
        /// </summary>
        /// <param name="sourceUri">A string that represents the source URI.</param>
        /// <param name="destUri">A string that represents the destination URI.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Copy(string sourceUri, string destUri);

        /// <summary>
        /// Creates a duplicate of the source resource identified by the source URI in the destination resource identified by the destination URI.
        /// </summary>
        /// <param name="sourceUri">The source <see cref="Uri"/>.</param>
        /// <param name="destUri">The destination <see cref="Uri"/>.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Copy(Uri sourceUri, Uri destUri);

        /// <summary>
        /// Creates a duplicate of the source resource identified by the source URI in the destination resource identified by the destination URI.
        /// </summary>
        /// <param name="sourceUri">A string that represents the source URI.</param>
        /// <param name="destUri">A string that represents the destination URI.</param>
        /// <param name="parameters">Parameters of the COPY operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Copy(string sourceUri, string destUri, CopyParameters parameters);

        /// <summary>
        /// Creates a duplicate of the source resource identified by the source URI in the destination resource identified by the destination URI.
        /// </summary>
        /// <param name="sourceUri">The source <see cref="Uri"/>.</param>
        /// <param name="destUri">The destination <see cref="Uri"/>.</param>
        /// <param name="parameters">Parameters of the COPY operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" /></returns>
        WebDavResponse Copy(Uri sourceUri, Uri destUri, CopyParameters parameters);

        /// <summary>
        /// Moves the resource identified by the source URI to the destination identified by the destination URI.
        /// </summary>
        /// <param name="sourceUri">A string that represents the source URI.</param>
        /// <param name="destUri">A string that represents the destination URI.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Move(string sourceUri, string destUri);

        /// <summary>
        /// Moves the resource identified by the source URI to the destination identified by the destination URI.
        /// </summary>
        /// <param name="sourceUri">The source <see cref="Uri"/>.</param>
        /// <param name="destUri">The destination <see cref="Uri"/>.</param>
        /// <returns>An instance of <see cref="WebDavResponse" /></returns>
        WebDavResponse Move(Uri sourceUri, Uri destUri);

        /// <summary>
        /// Moves the resource identified by the source URI to the destination identified by the destination URI.
        /// </summary>
        /// <param name="sourceUri">A string that represents the source URI.</param>
        /// <param name="destUri">A string that represents the destination URI.</param>
        /// <param name="parameters">Parameters of the MOVE operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Move(string sourceUri, string destUri, MoveParameters parameters);

        /// <summary>
        /// Moves the resource identified by the source URI to the destination identified by the destination URI.
        /// </summary>
        /// <param name="sourceUri">The source <see cref="Uri"/>.</param>
        /// <param name="destUri">The destination <see cref="Uri"/>.</param>
        /// <param name="parameters">Parameters of the MOVE operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Move(Uri sourceUri, Uri destUri, MoveParameters parameters);

        /// <summary>
        /// Takes out a shared lock or refreshes an existing lock of the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <returns>An instance of <see cref="LockResponse" />.</returns>
        LockResponse Lock(string requestUri);

        /// <summary>
        /// Takes out a shared lock or refreshes an existing lock of the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <returns>An instance of <see cref="LockResponse" />.</returns>
        LockResponse Lock(Uri requestUri);

        /// <summary>
        /// Takes out a lock of any type or refreshes an existing lock of the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="parameters">Parameters of the LOCK operation.</param>
        /// <returns>An instance of <see cref="LockResponse" />.</returns>
        LockResponse Lock(string requestUri, LockParameters parameters);

        /// <summary>
        /// Takes out a lock of any type or refreshes an existing lock of the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="parameters">Parameters of the LOCK operation.</param>
        /// <returns>An instance of <see cref="LockResponse" />.</returns>
        LockResponse Lock(Uri requestUri, LockParameters parameters);

        /// <summary>
        /// Removes the lock identified by the lock token from the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="lockToken">The resource lock token.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Unlock(string requestUri, string lockToken);

        /// <summary>
        /// Removes the lock identified by the lock token from the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="lockToken">The resource lock token.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Unlock(Uri requestUri, string lockToken);

        /// <summary>
        /// Removes the lock identified by the lock token from the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">A string that represents the request URI.</param>
        /// <param name="parameters">Parameters of the UNLOCK operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Unlock(string requestUri, UnlockParameters parameters);

        /// <summary>
        /// Removes the lock identified by the lock token from the resource identified by the request URI.
        /// </summary>
        /// <param name="requestUri">The <see cref="Uri"/> to request.</param>
        /// <param name="parameters">Parameters of the UNLOCK operation.</param>
        /// <returns>An instance of <see cref="WebDavResponse" />.</returns>
        WebDavResponse Unlock(Uri requestUri, UnlockParameters parameters);
    }
}