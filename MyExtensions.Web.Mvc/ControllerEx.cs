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
            int total = query.Count();

            if (page.HasValue == false)
            {
                page = 1;
            }

            if (pagesize.HasValue == false)
            {
                pagesize = 10;
            }

            int skip = (page.Value - 1) * pagesize.Value;

            var data = query.Skip(skip).Take(pagesize.Value);

            return new PagedList<T>(data.ToList(), page.Value, pagesize.Value, total);
        }

        protected virtual PagedList<T> GetPagedList<T>(IQueryable<T> query, int start, int limit, string sort, string dir)
where T : new()
        {
            Check.Assert(limit != 0);

            int page = 1;

            if (start != 0)
            {
                page = start / limit;

                if (start % limit != 0)
                {
                    page++;
                }
            }

            int total = query.Count();

            IQueryable<T> orderedQuery = null;

            if (sort.IsNullOrEmpty() == false)
            {
                if (dir.ToLower() == "desc")
                {
                    orderedQuery = query.ApplyOrderByDescendingClause<T>(sort);
                }
                else
                {
                    orderedQuery = query.ApplyOrderByClause<T>(sort);
                }
            }
            else
            {
                orderedQuery = query.AsQueryable();
            }

            int skip = start - 1;

            if (skip < 0)
                skip = 0;

            var data = orderedQuery.Skip(skip).Take(limit);

            return new PagedList<T>(data.ToList(), page, limit, total);
        }
        #endregion
    }
}
