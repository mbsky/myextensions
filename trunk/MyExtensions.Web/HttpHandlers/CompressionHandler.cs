
using System;
using System.IO;

namespace System.Web.HttpHandlers
{
    /// <summary>
    /// Compression HttpHandler
    /// </summary>
    public class CompressionHandler : System.Web.IHttpHandler
    {
        #region private object
        private const string GZIP = "gzip";
        private const string DEFLATE = "deflate";
        #endregion

        #region IHttpHandler
        /// <summary>
        /// true
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }
        /// <summary>
        /// process request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(System.Web.HttpContext context)
        {
            
            string requestpath = context.Request.FilePath.ToLower();

            string originpath = requestpath.Substring(0, requestpath.LastIndexOf("."));

            string filepath = context.Server.MapPath(originpath);

            if (originpath.EndsWith(".css"))
            {
                context.Response.ContentType = "text/css";
            }

            if (context.IsEncodingAccepted(GZIP))
            {
                string gz = filepath + ".gz";

                if (File.Exists(gz))
                {
                    context.SetEncoding(GZIP);

                    context.Response.WriteFile(gz);

                    return;
                }
            }
            
            if (context.IsEncodingAccepted(DEFLATE))
            {
                string de = filepath + ".de";

                if (File.Exists(de))
                {
                    context.SetEncoding(DEFLATE);

                    context.Response.WriteFile(de);

                    return;
                }
            }

            if (File.Exists(filepath))
            {
                context.Response.WriteFile(filepath);
            }
        }
        #endregion
    }
}