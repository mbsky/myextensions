using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace System.Web.Mvc
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class ViewPageEx : ViewPage, IViewDataContainerEx
    {
        public virtual bool EnableMasterTheming
        {
            get
            {
                if (this.Master != null)
                {
                    return AppSettings.GetBoolean("EnableMasterTheming");
                }

                return false;
            }
        }

        ThemesHelper _themesHelper;

        ThemesHelper ThemesHelper
        {
            get
            {
                if (null == _themesHelper)
                    _themesHelper = new ThemesHelper();

                return _themesHelper;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            if (EnableMasterTheming && !ThemesHelper.Theme.IsNullOrEmpty())
            {
                string masterName = base.MasterPageFile.ToLower();

                this.MasterPageFile = masterName.Replace(".master", "." + ThemesHelper.Theme + ".master");
            }

            base.OnPreInit(e);
        }

        public bool IsDebugging
        {
            get
            {
                if (ViewContext.Controller is ControllerEx)
                    return ((ControllerEx)ViewContext.Controller).IsDebugging;
                else
                    return false;
            }
        }

        private AppSettingsHelper appSettings;//= new AppSettingsHelper(WebConfigurationManager.AppSettings);

        public AppSettingsHelper AppSettings
        {
            get
            {
                if (null == appSettings)
                {
                    if (ViewContext.Controller is ControllerEx)
                        return ((ControllerEx)ViewContext.Controller).AppSettings;
                    else
                    {
                        appSettings = new AppSettingsHelper(WebConfigurationManager.AppSettings);
                    }             
                }

                return appSettings;            
            }
        }
    }

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public abstract class ViewPageEx<TModel> : ViewPageEx where TModel : class
    {

        private ViewDataDictionary<TModel> _viewData;

        public new AjaxHelper<TModel> Ajax
        {
            get;
            set;
        }

        public new HtmlHelper<TModel> Html
        {
            get;
            set;
        }

        public new TModel Model
        {
            get
            {
                return ViewData.Model;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public new ViewDataDictionary<TModel> ViewData
        {
            get
            {
                if (_viewData == null)
                {
                    SetViewData(new ViewDataDictionary<TModel>());
                }
                return _viewData;
            }
            set
            {
                SetViewData(value);
            }
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            Ajax = new AjaxHelper<TModel>(ViewContext, this);
            Html = new HtmlHelper<TModel>(ViewContext, this);
        }

        protected override void SetViewData(ViewDataDictionary viewData)
        {
            _viewData = new ViewDataDictionary<TModel>(viewData);

            base.SetViewData(_viewData);
        }
    }
}
