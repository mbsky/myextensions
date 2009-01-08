using System.ComponentModel;
using System.IO;

namespace System.Web
{
    public static class IHttpHandlerExtensions
    {
        #region RootPath
        /// <summary>
        /// return root path of site
        /// </summary>
        public static string RootPath(this IHttpHandler handler)
        {
            string path = HttpContext.Current.Request.ApplicationPath;
            return !path.EndsWith("/") ? string.Concat(path, "/") : path;
        }
        #endregion

        #region GetQueryParameter
        /// <summary>
        /// Gets the query parameter.
        /// </summary>
        /// <param name="queryParam">The query param.</param>
        /// <returns></returns>
        public static TType GetQueryParameter<TType>(this IHttpHandler handler, string queryParam)
        {
            return handler.GetQueryParameter<TType>(queryParam, default(TType));
        }

        /// <summary>
        /// Gets the query parameter.
        /// </summary>
        /// <param name="queryParam">The query param.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static TType GetQueryParameter<TType>(this IHttpHandler handler, string queryParam, TType defaultValue)
        {
            try
            {
                // Get the job id that is passed in on the query string.
                string strQuery = HttpContext.Current.Request.QueryString[queryParam];
                if (!string.IsNullOrEmpty(strQuery))
                {
                    TypeConverter returnConverter = TypeDescriptor.GetConverter(typeof(TType));
                    return (TType)returnConverter.ConvertFrom(strQuery);
                }
            }
            catch (Exception e)
            {
                // TODO: Log exception
                object a = e;
            }

            return defaultValue;
        }
        #endregion

        #region EnsureDataFoler
        /// <summary>
        /// 
        /// </summary>
        public static bool EnsureDataFoler(this IHttpHandler handler)
        {
            if (HttpContext.Current != null)
            {
                string folder = HttpContext.Current.Server.MapPath("~/App_Data/");

                if (!Directory.Exists(folder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(folder);

                    if (null != di)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        } 
        #endregion

    }
}
