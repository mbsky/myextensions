using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Specialized;

namespace System.Web.Mvc.Html
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class HtmlMessageExtensions
    {
        public static MessageModel GetMessage(this HtmlHelper html)
        {
            return html.GetMessage(null, null);
        }

        public static MessageModel GetMessage(this HtmlHelper html, string prefix)
        {
            return html.GetMessage(prefix, null);
        }

        public static MessageModel GetMessage(this HtmlHelper html, string prefix, FormModel form)
        {
            MessageModel msg = new MessageModel();

            ViewDataDictionary viewData = html.ViewDataContainer.ViewData;

            if (!viewData.ModelState.IsValid)
            {
                msg.Errors = viewData.ModelState.GetErrors(prefix);
            }

            if (null != form)
            {
                if (null != form.Notice && form.Notice.Count() != 0)
                {
                    // List<string> notice = new List<string>();

                    msg.Notice = form.Notice;
                }

                //todo:success

                if (null != viewData["Message"])
                {
                    msg.Success = new string[] { (string)viewData["Message"] };
                }
            }

            return msg;
        }
    }
}
