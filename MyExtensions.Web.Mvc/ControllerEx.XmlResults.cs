using System.Xml.Linq;

namespace System.Web.Mvc
{
    public abstract partial class ControllerEx
	{
        protected virtual XmlViewResult Xml()
        {
            return Xml("");
        }

        protected virtual XmlViewResult Xml(string viewName)
        {
            return new XmlViewResult(viewName);
        }

        protected virtual XDocumentResult Xml(XDocument document)
        {
            return new XDocumentResult(document);
        }
	}
}
