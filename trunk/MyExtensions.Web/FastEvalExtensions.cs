using System.Reflection;

namespace System.Web.UI
{
    public static class FastEvalExtensions
    {
        private static DynamicPropertyAccessorCache s_cache =
        new DynamicPropertyAccessorCache();

        public static object FastEval(this Control control, object o, string propertyName)
        {
            return s_cache.GetAccessor(o.GetType(), propertyName).GetValue(o);
        }

        public static object FastEval(this TemplateControl control, string propertyName)
        {
            return control.FastEval(control.Page.GetDataItem(), propertyName);
        }

    }
}
