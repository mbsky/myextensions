using System.Configuration;
using System.Web.Security;
using System.Globalization;

namespace System.Web.Mvc
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class ViewMasterPageEx : ViewMasterPage, IViewDataContainerEx
    {
        public bool IsDebugging
        {
            get
            {
                return (this as IViewDataContainerEx).GetIsDebugging();
            }
        }

        public AppSettingsHelper AppSettings
        {
            get
            {
                return (this as IViewDataContainerEx).GetAppSettings();
            }
        }
    }

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class ViewMasterPageEx<TModel> : ViewMasterPageEx where TModel : class
    {
        public new ViewDataDictionary<TModel> ViewData
        {
            get
            {
                ViewDataDictionary<TModel> viewData = new ViewDataDictionary<TModel>(base.ViewData);
                return viewData;
            }
        }
    }
}
