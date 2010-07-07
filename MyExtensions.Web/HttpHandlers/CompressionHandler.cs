
using System;
using System.IO;
using System.Globalization;

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

            string originpath = requestpath.Substring(0, requestpath.LastIndexOf(".")).ToLower();

            string filepath = context.Server.MapPath(originpath);

            if (originpath.EndsWith(".css"))
            {
                context.Response.ContentType = "text/css";
            }
            else if (originpath.EndsWith(".js"))
            {
                //context.Response.ContentType = "application/x-javascript";
                context.Response.ContentType = "text/javascript";
            }

            if (context.IsEncodingAccepted(GZIP))
            {
                string gz = filepath + ".gz";

                if (File.Exists(gz))
                {
                    context.SetEncoding(GZIP);

                    //context.Response.WriteFile(gz);

                    WriteFile(context, gz);

                    return;
                }
            }
            else if (context.IsEncodingAccepted(DEFLATE))
            {
                string de = filepath + ".de";

                if (File.Exists(de))
                {
                    context.SetEncoding(DEFLATE);

                    //context.Response.WriteFile(de);

                    WriteFile(context, de);

                    return;
                }
            }

            if (File.Exists(filepath))
            {
                WriteFile(context, filepath);
            }
        }

        private void WriteFile(HttpContext context, string _sourceFile)
        {
            var LastWriteTime = File.GetLastWriteTime(_sourceFile);

            if (!String.IsNullOrEmpty(context.Request.Headers["If-Modified-Since"]))
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                var lastMod = DateTime.ParseExact(context.Request.Headers["If-Modified-Since"], "r", provider).ToLocalTime();

                if (lastMod == LastWriteTime)
                {
                    context.Response.StatusCode = 304;
                    context.Response.StatusDescription = "Not Modified";
                    return;
                }
            }

            context.Response.Cache.SetLastModified(LastWriteTime);
            context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(60));
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetNoServerCaching();
            context.Response.WriteFile(_sourceFile);

            context.Response.End();
        }

        #endregion
    }
}