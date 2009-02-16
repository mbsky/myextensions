using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace System.Web
{
    public class ThemesHelper
    {
        AppSettingsHelper appSettings = new AppSettingsHelper();

        public virtual string[] Themes
        {
            get
            {
                return appSettings.GetArray("Themes");
            }
        }

        //public virtual string[] UserThemes
        //{
        //    get
        //    {
        //        return appSettings.GetArray("UserThemes");
        //    }
        //}

        //public virtual string[] MasterThemes
        //{
        //    get
        //    {
        //        return appSettings.GetArray("MasterThemes");
        //    }
        //}

        //public virtual string[] CssThemes
        //{
        //    get
        //    {
        //        return appSettings.GetArray("CssThemes");
        //    }
        //}

        public virtual string DefaultTheme
        {
            get
            {
                string theme = appSettings.Get("DefaultTheme", string.Empty);

                return theme;
            }
        }

        public virtual string Theme
        {
            get
            {
                string val = CookieHelper.GetCookieValue("theme");

                if (!val.IsNullOrEmpty())
                    return val;

                return DefaultTheme;
            }
            set
            {
                CookieHelper.SetCookie("theme", value, true);
            }
        }
    }
}
