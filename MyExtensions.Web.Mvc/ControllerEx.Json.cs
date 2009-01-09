using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;

namespace System.Web.Mvc
{
    public abstract partial class ControllerEx : Controller
    {
        // TODO DefaultJsonProvider AppSettings

        protected virtual JsonProvider DefaultJsonProvider
        {
            get
            {
                return JsonProvider.Microsoft;
            }
        }

        protected new virtual JsonResult Json(object data)
        {
            return Json(data, DefaultJsonProvider);
        }

        protected virtual JsonResult Json(object data, JsonSerializerSettings settings)
        {
            return Json(data, null, settings);
        }

        protected virtual JsonResult Json(object data, JsonProvider jsonProvider)
        {
            return Json(data, null /* contentType */, jsonProvider);
        }

        protected new JsonResult Json(object data, string contentType)
        {
            return Json(data, contentType, null /* contentEncoding */, DefaultJsonProvider);
        }

        protected virtual JsonResult Json(object data, string contentType, JsonProvider jsonProvider)
        {
            return Json(data, contentType, null /* contentEncoding */, jsonProvider);
        }

        protected virtual JsonResult Json(object data, string contentType, JsonSerializerSettings settings)
        {
            return Json(data, contentType, null /* contentEncoding */, settings);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return Json(data, contentType, contentEncoding, DefaultJsonProvider);
        }

        protected virtual JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonProvider jsonProvider)
        {
            /*
            if (!ViewData.ModelState.IsValid)
            {
                /// ToDo: write json errors
            }
            */
            return new JsonResultEx(jsonProvider)
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }

        protected virtual JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonSerializerSettings settings)
        {
            /*
            if (!ViewData.ModelState.IsValid)
            {
                /// ToDo: write json errors
            }
            */

            return new JsonResultEx(JsonProvider.Newtonsoft, settings)
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }
    }
}
