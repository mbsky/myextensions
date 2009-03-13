using System.Configuration;
using System.Web.Configuration;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public abstract partial class ControllerEx : Controller
    {
        protected virtual AutoResult Auto()
        {
            return new AutoResult(null);
        }

        protected virtual AutoResult Auto(string message)
        {
            Check.AssertNotNullOrEmpty(message, "message");
            return new AutoResult(message);
        }

        protected virtual AutoResult Auto(string message,string returnUrl)
        {
            Check.AssertNotNullOrEmpty(message, "message");
            Check.AssertNotNullOrEmpty(returnUrl, "returnUrl");
            return new AutoResult(message, returnUrl);
        }
    }
}
