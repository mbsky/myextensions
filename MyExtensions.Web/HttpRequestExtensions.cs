using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace System.Web
{
    public enum ResponseType
    {
        None,
        Html,
        Xml,
        Json,
    }

    public static class HttpRequestExtensions
    {

        #region GetPreferedResponseType
        public static ResponseType GetPreferedResponseType(this HttpRequest request)
        {
            ResponseType result = ResponseType.None;

            // The requested format 
            // is specified via the querystring
            string format = request.QueryString["format"];

            if (String.IsNullOrEmpty(format))
            {
                if (request.AcceptTypes != null &&
                    request.AcceptTypes.Length > 0 &&
                    !String.IsNullOrEmpty(request.AcceptTypes[0]) &&
                    request.AcceptTypes[0].IndexOf("json", StringComparison.InvariantCultureIgnoreCase) > 0)
                    result = ResponseType.Json;
                else
                    // If no querystring was specified, 
                    // assume the default HTML format
                    result = ResponseType.Html;
            }
            else
            {
                if (String.Equals(format, "html", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = ResponseType.Html;
                }
                else if (String.Equals(format, "xml", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = ResponseType.Xml;
                }
                else if (String.Equals(format, "json", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = ResponseType.Json;
                }
            }

            return result;
        } 
        #endregion

        #region IsAjaxRequest
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request != null && request.QueryString != null && request.Headers != null &&
                ("true".Equals(request.QueryString["__mvcajax"]) || !String.IsNullOrEmpty(request.Headers["Ajax"]) || "XMLHttpRequest".Equals(request.Headers["X-Requested-With"],
              StringComparison.InvariantCultureIgnoreCase));
        } 
        #endregion

        #region ExpandTildePath
        /// <summary>
        /// If the URL is is the format ~/SomePath, this 
        /// method expands the tilde using the app path.
        /// </summary>
        /// <param name="path"></param>
        public static string ExpandTildePath(this HttpRequest request, string path)
        {
            if (String.IsNullOrEmpty(path))
                return string.Empty;

            Check.Require(request, "request");

            string reference = path;
            if (reference.Substring(0, 2) == "~/")
            {
                string appPath = request.ApplicationPath;
                if (appPath == null)
                    appPath = string.Empty;
                if (appPath.EndsWith("/"))
                {
                    appPath = appPath.Left(appPath.Length - 1);
                }
                return appPath + reference.Substring(1);
            }
            return path;
        }
        #endregion

        #region BuildTraceDump
        /// <summary>
        /// Dumps the web request
        /// </summary>
        /// <param name="request">The request to dump.</param>
        /// <returns>A string with all the object information in it.</returns>
        public static string BuildTraceDump(this HttpRequest request)
        {
            Check.Require(request, "request");

            StringBuilder text = new StringBuilder();

            text.AppendFormat("HttpRequest Type:  {0}\n", request.GetType().ToString());
            text.AppendFormat("ContentLength:     {0}\n", request.ContentLength);
            text.AppendFormat("ContentType:       {0}\n", request.ContentType);
            text.AppendFormat("HttpMethod:        {0}\n", request.HttpMethod);
            text.AppendFormat("RawUrl:            {0}\n", request.RawUrl);

            return text.ToString();
        } 
        #endregion

        #region WebForwarder Methods
        /// <summary>
        /// Forwards the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="forwardUrl">The forward URL.</param>
        /// <param name="response">The response.</param>
        public static void Forward(this HttpRequest request, Uri forwardUrl, HttpResponse response)
        {
            // Setup forwarding request
            HttpWebRequest forwardRequest = request.GetForwardingRequest(forwardUrl);

            WebResponse forwardResponse = null;
            try
            {
                forwardResponse = forwardRequest.GetResponse();
            }
            catch (WebException e)
            {
                forwardResponse = e.Response;
            }

            CopyResponse(forwardResponse, response);
        }

        /// <summary>
        /// Gets the forwarding request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="forwardUrl">The forward URL.</param>
        /// <returns></returns>
        private static HttpWebRequest GetForwardingRequest(this HttpRequest request, Uri forwardUrl)
        {
            HttpWebRequest forwardRequest = (HttpWebRequest)HttpWebRequest.Create(forwardUrl);

            forwardRequest.ContentLength = request.ContentLength;
            forwardRequest.ContentType = request.ContentType;
            if (request.Cookies.Count > 0)
            {
                forwardRequest.CookieContainer = new CookieContainer();
                foreach (String inCookieName in request.Cookies)
                {
                    HttpCookie inCookie = request.Cookies[inCookieName];
                    Cookie outCookie = new Cookie(inCookie.Name, inCookie.Value, inCookie.Path, forwardRequest.RequestUri.Authority);
                    outCookie.Expires = inCookie.Expires;

                    forwardRequest.CookieContainer.Add(outCookie);
                }
            }

            forwardRequest.Method = request.HttpMethod;
            forwardRequest.UserAgent = request.UserAgent;

            int length = request.ContentLength;
            byte[] buffer = new byte[length];

            if (forwardRequest.Method == "POST")
            {
                request.InputStream.Read(buffer, 0, length);
                forwardRequest.GetRequestStream().Write(buffer, 0, length);
            }

            return forwardRequest;
        }

        /// <summary>
        /// Copies the response.
        /// </summary>
        /// <param name="forwardResponse">The forward response.</param>
        /// <param name="response">The response.</param>
        private static void CopyResponse(WebResponse forwardResponse, HttpResponse response)
        {
            response.ContentType = forwardResponse.ContentType;
            using (Stream stream = forwardResponse.GetResponseStream())
            {
                byte[] buffer = new byte[4096];
                int bytesRead = stream.Read(buffer, 0, 4096);
                while (bytesRead > 0)
                {
                    response.BinaryWrite(buffer);
                    bytesRead = stream.Read(buffer, 0, 4096);
                }
            }
        }

        #endregion
    }
}
