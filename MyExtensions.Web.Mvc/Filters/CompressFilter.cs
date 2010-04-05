using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.IO.Compression;

namespace System.Web.Mvc
{
    /// <summary>
    /// 适用于ASP.NET MVC的ActionFilter，用于将输出进行压缩。压缩方式取决于客户机浏览器支持的压缩类型。如果gzip和deflate均不支持则数据会不被压缩。
    /// </summary>
    public class CompressFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string acceptedEncoding = filterContext.HttpContext.Request.Headers["Accepted-Encoding"] ?? "";
            acceptedEncoding = acceptedEncoding.ToLowerInvariant();
            HttpResponseBase response = filterContext.HttpContext.Response;
            if(acceptedEncoding.Contains("gzip"))
            {
                response.AppendHeader("Content-Encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if(acceptedEncoding.Contains("deflate"))
            {
                response.AppendHeader("Content-Encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}
