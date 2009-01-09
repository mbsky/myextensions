using System.IO;
using System.Xml.Serialization;

namespace System.Web.Mvc
{
    /// <summary>
    ///  Serialize a object to use XmlSerializer and return a XML document.
    /// </summary>
    public class XmlResult : ActionResult
    {
        object Data { get; set; }

        Type DataType { get; set; }

        public XmlResult(object data, Type type)
        {
            Data = data;
            DataType = type;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = "text/xml";

            if (Data != null)
            {
                XmlSerializer serializer = new XmlSerializer(DataType);
                MemoryStream ms = new MemoryStream();
                serializer.Serialize(ms, Data);
                response.Write(System.Text.Encoding.UTF8.GetString(ms.ToArray()));
            }
        }
    }
}
