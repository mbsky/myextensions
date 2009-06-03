using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Collections.Specialized;
using Microsoft.Web.Mvc;
using System.Web;
using System.Web.Configuration;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Fields 排成多少列
    /// </summary>
    public enum FieldsColumn
    {
        One = 1,
        Two
    }

    [Serializable]
    public class FormModel : List<FieldBase>, System.Web.Mvc.Html.IActionCaller
    {

        public FormModel(string name, string title, params FieldBase[] fields)
            : this(name, title, FieldsColumn.One, fields)
        {
        }

        public FormModel(string name, string title, FieldsColumn fieldColumnCount, params FieldBase[] fields)
            : base(fields)
        {
            Check.AssertNotNullOrEmpty(name, "name");

            Check.AssertNotNullOrEmpty(title, "title");

            Name = name;

            Title = title;

            this.FieldsColumnCount = fieldColumnCount;

            Method = FormMethod.Post;
        }

        public virtual FamIcon Icon { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Ajax { get; set; }

        public virtual string ActionName { get; set; }

        public virtual string ControllerName { get; set; }

        public virtual FieldsColumn FieldsColumnCount { get; set; }

        /// <summary>
        /// 不渲染为Panel(面板样式) 将隐藏h1标题 消息和输入字段边框
        /// </summary>
        public virtual bool NotRenderAsPanel { get; set; }

        public virtual FormMethod Method { get; set; }

        public virtual string Title { get; set; }

        public virtual string Prefix { get; set; }

        public virtual string TemplateName { get; set; }

        /// <summary>
        /// 是否禁止跨域访问 prevent-cross-site-request-forgery-csrf
        /// </summary>
        public virtual bool AntiForgeryToken { get; set; }

        public virtual string[] Notice
        {
            get
            {
                return this.Messages.Notice;
            }
            set
            {
                this.Messages.Notice = value;
            }
        }

        private MessageModel messages;

        protected MessageModel Messages
        {
            get
            {
                if (null == messages)
                    messages = new MessageModel();

                return messages;
            }
        }
    }

    public static class FormModelExtensions
    {

        public static void RenderForm(this HtmlHelper helper, FormModel model)
        {
            helper.RenderForm(model, null);
        }

        static readonly string defaultTemplateName = "FormPanel";

        public static void RenderForm(this HtmlHelper helper, FormModel model, string prefix)
        {

            string templateName = model.TemplateName.IsNullOrEmpty() ? defaultTemplateName : model.TemplateName;

            if (model.ControllerName.IsNullOrEmpty())
            {
                model.ControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            }

            if (model.ActionName.IsNullOrEmpty())
            {
                model.ActionName = (string)helper.ViewContext.RouteData.Values["action"];
            }

            if (!prefix.IsNullOrEmpty())
            {
                templateName = prefix + "." + templateName;
            }

            helper.RenderPartial(templateName, model, helper.ViewDataContainer.ViewData);
        }
    }
}
