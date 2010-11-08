using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Configuration;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public abstract partial class ControllerEx : Controller
    {
        public ControllerEx()
            : base()
        {
            Routes = RouteTable.Routes;

            AppSettings = new AppSettingsHelper(WebConfigurationManager.AppSettings);
        }

        private log4net.ILog _log;

        protected virtual log4net.ILog log
        {
            get
            {
                if (null == _log)
                {
                    _log = log4net.LogManager.GetLogger(GetType());
                }
                return _log;
            }
        }

        public RouteCollection Routes { get; protected set; }
        public AppSettingsHelper AppSettings { get; protected set; }

        public bool IsDebugging { get; private set; }

        /// <summary>
        /// No Need To Create Action In Controller, Just CreateView
        /// http://weblogs.asp.net/stephenwalther/archive/2008/07/21/asp-net-mvc-tip-22-return-a-view-without-creating-a-controller-action.aspx
        /// </summary>
        protected override void HandleUnknownAction(string actionName)
        {
            this.View(actionName).ExecuteResult(this.ControllerContext);
        }

        public virtual string GetReferrerUrl
        {
            get
            {
                return Request.UrlReferrer == null ? "/" : Request.UrlReferrer.PathAndQuery;
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Oxite BaseController
#if DEBUG
                        IsDebugging = true;
#else
            IsDebugging = false;
#endif

            base.OnActionExecuted(filterContext);

        }

        public class CoolListRequest
        {
            private CoolListRequest()
            {
            }

            public CoolListRequest(HttpRequestBase Request)
            {
                start = Int32.Parse(Request["start"] != null ? Request["start"] : "0");
                limit = Int32.Parse(Request["limit"] != null ? Request["limit"] : "10");
                dir = Request["dir"] != null ? Request["dir"] : string.Empty;
                sort = Request["sort"] != null ? Request["sort"] : string.Empty;
                whereClause = Request["whereClause"] != null ? Request["whereClause"] : string.Empty;
            }

            public int start { get; set; }

            public int limit { get; set; }

            public string dir { get; set; }

            public string sort { get; set; }

            public string whereClause { get; set; }
        }

        #region PagedList

        protected virtual PagedList<T> GetPagedList<T>(IQueryable<T> query, int? page, int? pagesize)
where T : new()
        {
            return query.GetPagedList(page,pagesize);
        }

        protected virtual PagedList<T> GetPagedList<T>(IQueryable<T> query, int start, int limit, string sort, string dir)
where T : new()
        {
            return query.GetPagedList(start, limit, sort, dir);
        }
        #endregion
    }
}
