using System.Xml.Linq;

namespace System.Web.Mvc
{
    public abstract partial class ControllerEx
	{
        protected virtual XmlResult Xml(object data)
        {
            Check.AssertNotNullOrEmpty(data,"data");

            return new XmlResult(data, data.GetType());
        }

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
