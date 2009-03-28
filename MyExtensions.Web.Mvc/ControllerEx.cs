using System.Configuration;
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
    }
}
