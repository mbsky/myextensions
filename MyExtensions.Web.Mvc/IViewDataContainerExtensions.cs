using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Configuration;

namespace System.Web.Mvc
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public interface IViewDataContainerEx
    {
        ViewContext ViewContext { get;}
    }

    public static class IViewDataContainerExExtensions
    {
        public static bool GetIsDebugging(this IViewDataContainerEx view)
        {
            if (view.ViewContext.Controller is ControllerEx)
                return ((ControllerEx)view.ViewContext.Controller).IsDebugging;
            else
                return false;
        }

        public static AppSettingsHelper GetAppSettings(this IViewDataContainerEx view)
        {
            if (view.ViewContext.Controller is ControllerEx)
                return ((ControllerEx)view.ViewContext.Controller).AppSettings;
            else
                return null;
        }
    }
}
