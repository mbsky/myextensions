using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    public abstract partial class ControllerEx : Controller
	{
        protected virtual ActionResult Confirm(ConfirmOptionsModel option)
        {
            Check.AssertNotNullOrEmpty(option, "option");

            return View("Confirm", option);
        }
	}
}
