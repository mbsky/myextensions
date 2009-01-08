using System.ComponentModel;
using System.Reflection;

namespace System.Collections.Specialized
{
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static T CreateObject<T>(this NameValueCollection collection) where T : new()
        {
            T obj = new T();

            return collection.SetObjectValues<T>(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T SetObjectValues<T>(this NameValueCollection collection, T obj) where T : new()
        {
            foreach (string propName in collection.Keys)
            {
                PropertyInfo prop = obj.GetType().GetProperty(propName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (prop != null)
                {
                    string strValue = collection[propName];
                    object value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromString(strValue);
                    prop.SetValue(obj, value, null);
                }
            }

            return obj;
        }
    }
}
