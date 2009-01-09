using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace System.Web.Mvc
{
    /// <summary>
    /// read a Image Data from a Image or grab from a url and return jpeg image data. 
    /// </summary>
    public class ImageResult : ActionResult
    {
        public Image imageData;

        public ImageResult(Image image)
        {
            imageData = image;
        }

        public ImageResult(string imgUrl)
        {
            WebRequest req = WebRequest.Create(imgUrl);
            WebResponse res = req.GetResponse();
            Stream resStream = res.GetResponseStream();
            imageData = Image.FromStream(resStream);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            // SET HTTP Header
            response.ContentType = "image/jpeg";

            // Response
            imageData.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
        }

    }
}
