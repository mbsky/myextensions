using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;

namespace System.Web.Mvc.Html
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class HtmlCheckBoxExtensions
    {

        public static string CheckBox(this HtmlHelper helper, string name, string text, string value, bool isChecked)
        {
            //var setHash = htmlAttributes.ToAttributeList();

            string attributeList = string.Empty;

            //if (setHash != null)
            //    attributeList = setHash;

            return string.Format("<input id=\"{0}\" name=\"{0}\" value=\"{1}\" {2} type=\"checkbox\" {4}/>{3}",
                name, value, isChecked ? "checked=\"checked\"" : "",
                string.IsNullOrEmpty(text) ? "" : string.Format("<label id=\"{0}\" for=\"{1}\">{2}</label>", name + "_label", name, text),
                attributeList);
        }

    }
}
