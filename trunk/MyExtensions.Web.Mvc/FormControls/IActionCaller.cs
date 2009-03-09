using System;

namespace System.Web.Mvc.Html
{
    public interface IActionCaller
    {
        string ActionName { get; set; }
        bool Ajax { get; set; }
        bool AntiForgeryToken { get; set; }
        string ControllerName { get; set; }
        System.Web.Mvc.FormMethod Method { get; }
    }
}
