using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// 仅根据客户机上次访问的时间戳进行简单缓存处理
    /// </summary>
    public class LazyCacheFilter : ActionFilterAttribute
    {
        #region Properties
        public long Seconds = 60;
        public TimeSpan MaxAge = new TimeSpan(1, 0, 0, 0);
        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            string sinceTag = request.Headers["If-Modified-Since"];
            if (sinceTag != null &&
                TimeSpan.FromTicks(DateTime.Now.Ticks - DateTime.Parse(sinceTag).Ticks).Seconds < Seconds)
            {
                response.StatusCode = 304;
                response.StatusDescription = "Not Modified";
            }
            else
                setClientCaching(filterContext.HttpContext, DateTime.Now);
        }

        #region private Helper Methods
        private void setClientCaching(HttpContextBase context, DateTime time)
        {
            var cache = context.Response.Cache;
            cache.SetETag(time.Ticks.ToString());//firefox发出的request都有ETag？
            cache.SetLastModified(time);
            cache.SetMaxAge(MaxAge);
            cache.SetSlidingExpiration(true);
            cache.SetCacheability(HttpCacheability.Public);
        }
        #endregion
    }
}
