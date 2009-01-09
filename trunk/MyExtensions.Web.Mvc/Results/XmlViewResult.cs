
namespace System.Web.Mvc
{
    /// <summary>
    /// return a ViewResult with "text/xml" ContentType,this result requires a friendly XmlTempalte ViewPage.
    /// </summary>
    public class XmlViewResult : ViewResult
    {
        private string viewName;

        public XmlViewResult() : this("") { }

        public XmlViewResult(string viewName)
            : base()
        {
            this.viewName = viewName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (!string.IsNullOrEmpty(viewName))
                ViewName = viewName;

            TempData = context.Controller.TempData;
            ViewData = context.Controller.ViewData;

            base.ExecuteResult(context);

            context.HttpContext.Response.ContentType = "text/xml";
        }
    }
}
