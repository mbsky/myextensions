using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;
using Microsoft.Web.Mvc;

namespace System.Web.Mvc.Html {

    public class HeadLink
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public object HtmlAttributes { get; set; }
    }

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class HtmlHelperExtensions {

        #region ActionImage

        public static string ActionImage<T>(this HtmlHelper html, Expression<Action<T>> action, string imageRelativeUrl, string alt, object imageAttributes)
     where T : Controller {
            string image = html.Image(imageRelativeUrl, alt, imageAttributes);
            return string.Format("<a href=\"{0}\">{1}</a>",
                html.BuildUrlFromExpression<T>(action),
                image);
        } 
        #endregion

        #region Label

        public static string Label(this HtmlHelper helper, string text) {
            return String.Format("<label>{0}</label>", text);
        }

        public static string Label(this HtmlHelper helper, string text, string @for) {
            return String.Format("<label for=\"{0}\">{1}</label>", @for, text);
        }
        #endregion

        #region ConditionalLink
        public static string ConditionalLink(this HtmlHelper html, string url, string text) {
            return ConditionalLink(html, url, text, null /* htmlAttributes */);
        }

        public static string ConditionalLink(this HtmlHelper html, string url, string text, object htmlAttributes) {
            Check.AssertNotNullOrEmpty(text, "text");

            string encodedText = html.Encode(text);

            if (String.IsNullOrEmpty(url))
                return encodedText;

            var tag = new TagBuilder("a") { InnerHtml = encodedText };
            tag.Attributes["href"] = url;

            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return tag.ToString();
        }
        #endregion

        #region HeadLink

        public static string HeadLink(this HtmlHelper htmlHelper, HeadLink headLink) {
            return htmlHelper.HeadLink(headLink.Rel, headLink.Href, headLink.Type, headLink.Title, headLink.HtmlAttributes);
        }

        public static string HeadLink(this HtmlHelper htmlHelper, string rel, string href, string type, string title) {
            return htmlHelper.HeadLink(rel, href, type, title, null);
        }

        public static string HeadLink(this HtmlHelper htmlHelper, string rel, string href, string type, string title, object htmlAttributes) {
            TagBuilder tagBuilder = new TagBuilder("link");

            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (!string.IsNullOrEmpty(rel))
                tagBuilder.MergeAttribute("rel", rel);
            if (!string.IsNullOrEmpty(href))
                tagBuilder.MergeAttribute("href", href);
            if (!string.IsNullOrEmpty(type))
                tagBuilder.MergeAttribute("type", type);
            if (!string.IsNullOrEmpty(title))
                tagBuilder.MergeAttribute("title", title);

            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        #endregion

        #region Input

        public static string DropDownList(this HtmlHelper htmlHelper, string name, SelectList selectList, object htmlAttributes, bool isEnabled) {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled) {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.AppendLine(htmlHelper.DropDownList(string.Format("{0}_view", name), selectList, htmlAttributeDictionary));
                inputItemBuilder.AppendLine(htmlHelper.Hidden(name, selectList.SelectedValue));
                return inputItemBuilder.ToString();
            }

            return htmlHelper.DropDownList(name, selectList, htmlAttributeDictionary);
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked, object htmlAttributes, bool isEnabled) {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled) {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.AppendLine(htmlHelper.RadioButton(string.Format("{0}_view", name), value, isChecked, htmlAttributeDictionary));
                if (isChecked)
                    inputItemBuilder.AppendLine(htmlHelper.Hidden(name, value));
                return inputItemBuilder.ToString();
            }

            return htmlHelper.RadioButton(name, value, isChecked, htmlAttributeDictionary);
        }

        public static string TextBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes, bool isEnabled) {
            RouteValueDictionary htmlAttributeDictionary = new RouteValueDictionary(htmlAttributes);

            if (!isEnabled) {
                htmlAttributeDictionary["disabled"] = "disabled";

                StringBuilder inputItemBuilder = new StringBuilder();
                inputItemBuilder.Append(htmlHelper.TextBox(string.Format("{0}_view", name), value, htmlAttributeDictionary));
                inputItemBuilder.Append(htmlHelper.Hidden(name, value));
                return inputItemBuilder.ToString();
            }

            return htmlHelper.TextBox(name, value, htmlAttributeDictionary);
        }

        public static string Button(this HtmlHelper htmlHelper, string name, string buttonContent, object htmlAttributes) {
            return htmlHelper.Button(name, buttonContent, new RouteValueDictionary(htmlAttributes));
        }

        public static string Button(this HtmlHelper htmlHelper, string name, string buttonContent, IDictionary<string, object> htmlAttributes) {
            TagBuilder tagBuilder = new TagBuilder("button") {
                InnerHtml = buttonContent
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region Link

        public static string Link(this HtmlHelper htmlHelper, string linkText, string href) {
            return Link(htmlHelper, linkText, href, null);
        }

        public static string Link(this HtmlHelper htmlHelper, string linkText, string href, object htmlAttributes) {
            return htmlHelper.Link(linkText, href, new RouteValueDictionary(htmlAttributes));
        }

        public static string Link(this HtmlHelper htmlHelper, string linkText, string href, IDictionary<string, object> htmlAttributes) {
            TagBuilder tagBuilder = new TagBuilder("a") {
                InnerHtml = linkText
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", href);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region ScriptBlock

        public static string ScriptBlock(this HtmlHelper htmlHelper, string type, string src) {
            return htmlHelper.ScriptBlock(type, src, null);
        }

        public static string ScriptBlock(this HtmlHelper htmlHelper, string type, string src, object htmlAttributes) {
            TagBuilder tagBuilder = new TagBuilder("script");

            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (!string.IsNullOrEmpty(type))
                tagBuilder.MergeAttribute("type", type);
            if (!string.IsNullOrEmpty(src))
                tagBuilder.MergeAttribute("src", src);

            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region SubmitLink
        public static string SubmitLink(this HtmlHelper html, string text, string formName)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.Attributes["href"] = "#";
            builder.Attributes["class"] = "submit";
            builder.Attributes["onclick"] = String.Format("document.{0}.submit(); return false;", formName);
            builder.InnerHtml = html.Encode(text);
            return builder.ToString();
        }

        public static string SubmitLink(this HtmlHelper html, string text, string formName, string confirmationText)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.Attributes["href"] = "#";
            builder.Attributes["class"] = "submit";
            builder.Attributes["onclick"] = String.Format("if (confirm('{0}')) {{ document.{1}.submit(); }} return false;", confirmationText.Replace("'", "\\'"), formName);
            builder.InnerHtml = html.Encode(text);
            return builder.ToString();
        }

        #endregion

        #region UnorderedList

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, Func<T, string> generateContent) {
            return UnorderedList<T>(htmlHelper, items, (t, i) => generateContent(t));
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, Func<T, string> generateContent, string cssClass) {
            return UnorderedList<T>(htmlHelper, items, (t, i) => generateContent(t), cssClass, null, null);
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, Func<T, int, string> generateContent) {
            return UnorderedList<T>(htmlHelper, items, generateContent, null);
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, Func<T, int, string> generateContent, string cssClass) {
            return UnorderedList<T>(htmlHelper, items, generateContent, cssClass, null, null);
        }

        public static string UnorderedList<T>(this HtmlHelper htmlHelper, IEnumerable<T> items, Func<T, int, string> generateContent, string cssClass, string itemCssClass, string alternatingItemCssClass) {
            if (items == null || items.Count() == 0) return "";

            StringBuilder sb = new StringBuilder(100);
            int counter = 0;

            sb.Append("<ul");
            if (!string.IsNullOrEmpty(cssClass))
                sb.AppendFormat(" class=\"{0}\"", cssClass);
            sb.Append(">");
            foreach (T item in items) {
                StringBuilder sbClass = new StringBuilder(40);

                if (counter == 0)
                    sbClass.Append("first ");
                if (item.Equals(items.Last()))
                    sbClass.Append("last ");

                if (counter % 2 == 0 && !string.IsNullOrEmpty(itemCssClass))
                    sbClass.AppendFormat("{0} ", itemCssClass);
                else if (counter % 2 != 0 && !string.IsNullOrEmpty(alternatingItemCssClass))
                    sbClass.AppendFormat("{0} ", alternatingItemCssClass);

                sb.Append("<li");
                if (sbClass.Length > 0)
                    sb.AppendFormat(" class=\"{0}\"", sbClass.Remove(sbClass.Length - 1, 1));
                sb.AppendFormat(">{0}</li>", generateContent(item, counter));

                counter++;
            }
            sb.Append("</ul>");

            return sb.ToString();
        }

        #endregion
    }
}
