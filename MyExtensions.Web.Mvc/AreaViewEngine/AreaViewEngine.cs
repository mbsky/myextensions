﻿using System.Web.Routing;

namespace System.Web.Mvc
{
    public class AreaViewEngine : WebFormViewEngine
    {
        public AreaViewEngine()
            : base()
        {
            /*
            old: 
            
             MasterLocationFormats = new[] {
                "~/Views/{1}/{0}.master",
                "~/Views/Shared/{0}.master"
            };

            ViewLocationFormats = new[] {
                "~/Views/{1}/{0}.aspx",
                "~/Views/{1}/{0}.ascx",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Shared/{0}.ascx"
            };
             
             */

            ViewLocationFormats = new[] { 
                "~/{0}.aspx",
                "~/{0}.ascx",
                "~/Views/{1}/{0}.aspx",
                "~/Views/{1}/{0}.ascx",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Shared/{0}.ascx",
            };

            MasterLocationFormats = new[] {
                "~/{0}.master",
                "~/Shared/{0}.master",
                "~/Views/{1}/{0}.master",
                "~/Views/Shared/{0}.master",
            };

            PartialViewLocationFormats = ViewLocationFormats;
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName)
        {

            ViewEngineResult areaResult = null;

            RouteValueDictionary dic = controllerContext.RouteData.Values;

            bool needToFindInAreas = dic.ContainsKey("noarea") == false && dic.ContainsKey("area");

            if (needToFindInAreas)
            {
                //try to find the area-view
                string areaPartialName = FormatViewName(controllerContext, partialViewName);
                areaResult = base.FindPartialView(controllerContext, areaPartialName);
                if (areaResult != null && areaResult.View != null)
                {
                    return areaResult;
                }

                // if not found,try to find the shared area-view

                string sharedAreaPartialName = FormatSharedViewName(controllerContext, partialViewName);
                areaResult = base.FindPartialView(controllerContext, sharedAreaPartialName);
                if (areaResult != null && areaResult.View != null)
                {
                    return areaResult;
                }
            }
            // if area-view not found , call base method.
            return base.FindPartialView(controllerContext, partialViewName);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName)
        {

            ViewEngineResult areaResult = null;

            RouteValueDictionary dic = controllerContext.RouteData.Values;

            bool needToFindInAreas = dic.ContainsKey("noarea") == false && dic.ContainsKey("area");

            if (needToFindInAreas)
            {
                string areaViewName = FormatViewName(controllerContext, viewName);
                areaResult = base.FindView(controllerContext, areaViewName, masterName);
                if (areaResult != null && areaResult.View != null)
                {
                    return areaResult;
                }
                string sharedAreaViewName = FormatSharedViewName(controllerContext, viewName);
                areaResult = base.FindView(controllerContext, sharedAreaViewName, masterName);
                if (areaResult != null && areaResult.View != null)
                {
                    return areaResult;
                }
            }

            return base.FindView(controllerContext, viewName, masterName);
        }

        private static string FormatViewName(ControllerContext controllerContext, string viewName)
        {
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string area = controllerContext.RouteData.Values["area"].ToString();
            return "Areas/" + area + "/Views/" + controllerName + "/" + viewName;
        }

        private static string FormatSharedViewName(ControllerContext controllerContext, string viewName)
        {
            string area = controllerContext.RouteData.Values["area"].ToString();

            return "Areas/" + area + "/Views/Shared/" + viewName;
        }
    }
}