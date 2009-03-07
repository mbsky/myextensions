using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Web.Mvc
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ModelStateDictionaryExtensions
    {
        public static string[] GetErrors(this ModelStateDictionary modelState)
        {
            return modelState.GetErrors(null);
        }

        public static string[] GetErrors(this ModelStateDictionary modelState, string prefix)
        {
            if (!modelState.IsValid)
            {
                List<string> errors = new List<string>();

                IEnumerable<KeyValuePair<string, ModelState>> states =
                    prefix.IsNullOrEmpty() ? modelState.AsEnumerable()
                    : modelState.Where(x => x.Key.StartsWith(prefix + "."));

                foreach (var modelStateKvp in states)
                {
                    foreach (var modelError in modelStateKvp.Value.Errors)
                    {
                        errors.Add(modelError.ErrorMessage);
                    }
                }

                return errors.ToArray();
            }

            return null;
        }
    }
}
