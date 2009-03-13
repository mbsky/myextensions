using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    public class AutoResult : ActionResult
    {
        public string Message;

        public string ReloadUrl;

        public AutoResult(string message)
            : this(message, null)
        {
        }

        public AutoResult(string message, string reloadUrl)
        {
            Message = message;
            ReloadUrl = reloadUrl;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpRequestBase request = context.HttpContext.Request;

            ControllerBase controller = context.Controller;

            ViewDataDictionary ViewData = controller.ViewData;

            ModelStateDictionary modelState = ViewData.ModelState;

            if (request.IsAjaxRequest())
            {

                JsonProvider provider = JsonProvider.Microsoft;

                if (controller is ControllerEx)
                {
                    provider = ((ControllerEx)controller).DefaultJsonProvider;
                }

                JsonResultEx jr = new JsonResultEx(provider)
                {
                };

                if (!modelState.IsValid)
                {
                    jr.Data = new { Msg = Message, Errors = modelState.GetErrors(), ContainerId = request.Form["formContainerId"] };
                }
                else
                {
                    jr.Data = new { Msg = Message };
                }

                jr.ExecuteResult(context);

                return;
            }

            if (ReloadUrl.IsNullOrEmpty() == false)
            {
                HttpResponseBase response = context.HttpContext.Response;

                response.Redirect(ReloadUrl);

                return;
            }

            ViewData["Message"] = Message;

            ViewResult vr = new ViewResult();

            vr.ViewData = ViewData;

            vr.ExecuteResult(context);
        }
    }
}
