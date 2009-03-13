using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public class ActionButton : BaseActionCaller
    {

        public ActionButton(string value)
        {
            Check.AssertNotNullOrEmpty(value, "value");
            Value = value;
        }

        //public virtual object RouteValues { get; set; }

        public virtual string Value { get; set; }

        public override FormMethod Method
        {
            get
            {
                return FormMethod.Post;
            }
        }
    }

    public static class ActionButtonExtensions
    {

        static readonly string defaultTemplateName = "ActionButton";

        public static void RenderActionButton(this HtmlHelper helper, ActionButton model)
        {

            string templateName = defaultTemplateName;

            if (model.ControllerName.IsNullOrEmpty())
            {
                model.ControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            }

            if (model.ActionName.IsNullOrEmpty())
            {
                model.ActionName = (string)helper.ViewContext.RouteData.Values["action"];
            }

            helper.RenderPartial(templateName, model, helper.ViewDataContainer.ViewData);
        }
    }
}
