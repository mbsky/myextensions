using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace System.Web.Mvc.Html
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class HtmlIconExtension {

        #region Icon
        /// <summary>
        /// render a icon http://www.FamFamFam.com
        /// </summary>
        /// <param name="html"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static string Icon(this HtmlHelper html, FamIcon icon) {

            return Icon(icon);
        }

        private static string ContentIconPath = System.Web.Configuration.WebConfigurationManager.AppSettings["ContentIconPath"];

        /// <summary>
        /// render a icon http://www.FamFamFam.com
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static string Icon(FamIcon icon)
        {

            if (icon == FamIcon.None)
            {
                return string.Empty;
            }

            string dir = ContentIconPath.IsNullOrEmpty() ? "/Content/icons/" : ContentIconPath;

            if (dir.EndsWith("/") == false)
            {
                dir += "/";
            }

            string path = dir + icon.ToString().ToCharacterSeparatedFileName('_', "png");
            //string style = string.Format("background-image:url({0}) !important",path);
            return string.Format("<img alt=\"\" class=\"icon\" src=\"{0}\" />", path);
        }

        #endregion
    }
}
