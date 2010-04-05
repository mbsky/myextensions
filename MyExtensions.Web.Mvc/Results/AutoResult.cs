using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    public class AutoResult : ActionResult
    {

        public enum ReloadAction : byte
        {
            AjaxLoad = 0,
            Redirect
        }

        public string Message;

        public string ReloadUrl;

        public ReloadAction ReloadOption { get; set; }

        public AutoResult(string message)
            : this(message, null)
        {
        }

        public AutoResult(string message, string reloadUrl):this(message,reloadUrl,ReloadAction.AjaxLoad)
        {
        }

        public AutoResult(string message, string reloadUrl, ReloadAction reloadOption)
        {
            Message = message;
            ReloadUrl = reloadUrl;
            ReloadOption = reloadOption;
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
                    jr.Data = new { Msg = Message, Errors = modelState.GetErrors(), ContainerId = request.Form["formContainerId"], ReloadUrl = ReloadUrl, ReloadOption = (byte)ReloadOption };
                }
                else
                {
                    jr.Data = new { Msg = Message, ReloadUrl = ReloadUrl, ReloadOption = (byte)ReloadOption };
                }

                jr.ExecuteResult(context);

                return;
            }

            if (ReloadUrl.IsNullOrEmpty() == false && ReloadOption == ReloadAction.Redirect)
            {
                HttpResponseBase response = context.HttpContext.Response;

                response.Redirect(ReloadUrl);

                return;
            }

            ViewData["Message"] = Message;

            ViewResult vr = new ViewResult()
            {
                ViewData = controller.ViewData,
                ViewName = "Message"
            };

            vr.ExecuteResult(context);
        }
    }
}
