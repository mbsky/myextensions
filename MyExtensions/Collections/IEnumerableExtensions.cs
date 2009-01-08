using System.Text;

namespace System.Collections
{
    public static class IEnumerableExtensions
    {
        public static string Concat(this IEnumerable items, string separator)
        {
            return Concat(items, separator, "{0}");
        }

        public static string Concat(this IEnumerable items, string separator, string template)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj2 in items)
            {
                builder.Append(separator);
                builder.Append(string.Format(template, obj2.ToString()));
            }
            return builder.ToString().RightOf(separator);
        }


    }
}
