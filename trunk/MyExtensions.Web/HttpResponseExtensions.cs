using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web
{
    public static class HttpResponseExtensions
    {
        #region BuildTraceDump
        /// <summary>
        /// Dumps the web response 
        /// </summary>
        /// <param name="response">The response to dump.</param>
        /// <returns>A string with all the object information in it.</returns>
        public static string BuildTraceDump(this HttpResponse response)
        {
            StringBuilder text = new StringBuilder();

            text.AppendFormat("HttpResponse Type:  {0}\n", response.GetType().ToString());
            text.AppendFormat("ContentType:       {0}\n", response.ContentType);
            text.AppendFormat("StatusCode:        {0}\n", response.StatusCode);
            text.AppendFormat("Status:            {0}\n", response.Status);
            text.AppendFormat("StatusDescription: {0}\n", response.Status);

            return text.ToString();
        } 
        #endregion
    }
}
