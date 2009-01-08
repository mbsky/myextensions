using System.IO.Compression;
using System.Web;
using System;

namespace System.Web
{
    public static class HttpContextExtensions
    {
        #region IsConnectingLocally
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsConnectingLocally(this HttpContext context)
        {
            Check.Require(context, "context");

            return String.Equals(context.Request.Url.Host, "localhost", StringComparison.InvariantCultureIgnoreCase)
                && context.GetUserIpAddress() == HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]
                && context.Request.UserHostAddress == "127.0.0.1";
        } 
        #endregion

        #region IsEncodingAccepted
        public static bool IsEncodingAccepted(this HttpContext context, string encoding)
        {
            if (context != null)
                return context.Request.Headers["Accept-encoding"] != null && context.Request.Headers["Accept-encoding"].Contains(encoding);
            return false;
        }

        public static void SetEncoding(this System.Web.HttpContext context, string encoding)
        {
            if (context != null)
                context.Response.AppendHeader("Content-encoding", encoding);
        } 
        #endregion

        #region HttpCompressable

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool HttpCompressable(this HttpContext context)
        {
            Check.Require(context, "context");

            HttpRequest request = context.Request;

            string ua = request.UserAgent != null ? request.UserAgent.ToLowerInvariant() : "";

            return !ua.Contains("konqueror") && !ua.Contains("safari");
        }

        #endregion

        #region ExecuteHttpCompression
        /// <summary>
        ///  ExecuteHttpCompression must be userd in Page's OnLoad event
        /// </summary>
        public static void ExecuteHttpCompression(this HttpContext context)
        {
            if (context.HttpCompressable())
            {
                HttpResponse response = context.Response;

                if (context.IsEncodingAccepted("gzip"))
                {
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress, true);
                    response.AppendHeader("Content-encoding", "gzip");
                    response.AppendHeader("Vary", "Content-encoding");
                    //Response.Write("HTTP Compression Enabled (GZip)");
                }
                else if (context.IsEncodingAccepted("deflate"))
                {
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress, true);
                    response.AppendHeader("Content-encoding", "deflate");
                    response.AppendHeader("Vary", "Content-encoding");
                    //Response.Write("HTTP Compression Enabled (Deflate)");
                }
            }
        }
        #endregion

        #region GetUserIpAddress
        /// <summary>
        /// Returns the IP Address of the user making the current request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static string GetUserIpAddress(this HttpContext context)
        {
            if (context == null) return string.Empty;

            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (String.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                // Requests behind a proxy might contain multiple IP 
                // addresses in the forwarding header.
                if (result.IndexOf(',') > 0)
                {
                    result = result.LeftBefore(",");
                }
            }

            return result;
        } 
        #endregion
    }
}
