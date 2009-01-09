using System.Xml.Linq;

namespace System.Web.Mvc
{
    /// <summary>
    /// return a XDocument result to download.
    /// </summary>
    public class XDocumentResult : ActionResult
    {
        private XDocument document;

        public XDocumentResult(XDocument document)
            : base()
        {
            this.document = document;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            document.Save(context.HttpContext.Response.Output, SaveOptions.DisableFormatting);
        }
    }
}
