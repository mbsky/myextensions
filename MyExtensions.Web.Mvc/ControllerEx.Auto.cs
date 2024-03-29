﻿using System.Configuration;
using System.Web.Configuration;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public abstract partial class ControllerEx : Controller
    {
        protected virtual AutoResult Auto()
        {
            return new AutoResult(null);
        }

        protected virtual AutoResult Auto(string message)
        {
            Check.AssertNotNullOrEmpty(message, "message");
            return new AutoResult(message);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="message"></param>
        /// <param name="returnUrl"></param>
        /// <remarks>Note : You should override this method in your baseController inherited from ControllerEx to write your custom ViewData</remarks>
        /// <returns></returns>
        protected virtual AutoResult Auto(string message, string returnUrl)
        {
            return Auto(message, returnUrl, AutoResult.ReloadAction.AjaxLoad);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="message"></param>
        /// <param name="returnUrl"></param>
        /// <remarks>Note : You should override this method in your baseController inherited from ControllerEx to write your custom ViewData</remarks>
        /// <returns></returns>
        protected virtual AutoResult Auto(string message, string returnUrl, AutoResult.ReloadAction reloadOption)
        {
            Check.AssertNotNullOrEmpty(message, "message");
            Check.AssertNotNullOrEmpty(returnUrl, "returnUrl");

            return new AutoResult(message, returnUrl, reloadOption);
        }
    }
}
